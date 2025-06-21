using Eventix.Modules.Ticketing.Domain.Events.DomainEvents;
using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.Domain.Events.ValueObjects;
using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.Responses;
using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Ticketing.Domain.Events.Entities
{
    public sealed class Event : Entity, IAggregateRoot
    {
        private Event(string title, string description, Location location, DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            Specification = (title, description);
            DateRange = (startsAtUtc, endsAtUtc);
            Location = location;
            IsCanceled = false;
            Validate();
        }

        private Event()
        { }

        public EventSpecification Specification { get; private set; } = default!;
        public Location? Location { get; private set; } = default!;
        public DateRange DateRange { get; private set; } = default!;
        public bool IsCanceled { get; private set; }

        public static Result<Event> Create(string title, string description, Location location, DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            if (endsAtUtc.HasValue && endsAtUtc < startsAtUtc)
                return Result.Failure<Event>(EventErrors.EndDatePrecedesStartDate);

            return new Event(title, description, location, startsAtUtc, endsAtUtc);
        }

        public void Reschedule(DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            if (DateRange.StartsAtUtc == startsAtUtc
                && DateRange.EndsAtUtc == endsAtUtc) return;

            DateRange = (startsAtUtc, endsAtUtc);

            Raise(new EventRescheduledDomainEvent(Id, DateRange.StartsAtUtc, DateRange.EndsAtUtc));
        }

        public Result Cancel()
        {
            if (IsCanceled)
                return Result.Failure(EventErrors.AlreadyCanceled);

            IsCanceled = true;

            Raise(new EventCancelledDomainEvent(Id));

            return Result.Success();
        }

        public void PaymentsRefunded()
        {
            Raise(new EventPaymentsRefundedDomainEvent(Id));
        }

        public void TicketsArchived()
        {
            Raise(new EventTicketsArchivedDomainEvent(Id));
        }

        protected override void Validate()
        {
            AssertionConcern
                .EnsureNotNull(Specification, EventErrors.SpecificationIsRequired.Description);

            AssertionConcern
                .EnsureNotNull(DateRange, EventErrors.DateRangeIsRequired.Description);
        }
    }
}