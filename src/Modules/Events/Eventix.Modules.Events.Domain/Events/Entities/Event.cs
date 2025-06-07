using Eventix.Modules.Events.Domain.Events.DomainEvents;
using Eventix.Modules.Events.Domain.Events.Enumerators;
using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.Events.ValueObjects;
using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Domain.Events.Entities
{
    public sealed class Event : Entity
    {
        private Event(string title, string description, Guid categoryId, DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            Specification = (title, description);
            DateRange = (startsAtUtc, endsAtUtc);
            Status = EventStatusEnum.Draft;
            CategoryId = categoryId;
        }

        private Event()
        { }

        public EventSpecification Specification { get; private set; } = default!;
        public Location? Location { get; private set; } = default!;
        public DateRange DateRange { get; private set; } = default!;
        public EventStatusEnum Status { get; private set; }
        public Guid CategoryId { get; private set; }

        public static Result<Event> Create(string title, string description, Guid categoryId, DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            if (endsAtUtc.HasValue && endsAtUtc < startsAtUtc)
            {
                return Result.Failure<Event>(EventErrors.EndDatePrecedesStartDate);
            }
            var @event = new Event(title, description, categoryId, startsAtUtc, endsAtUtc);

            @event.Raise(new EventCreatedDomainEvent(@event.Id));

            return @event;
        }

        public Result Publish()
        {
            if (Status != EventStatusEnum.Draft)
            {
                return Result.Failure(EventErrors.NotDraft);
            }

            Status = EventStatusEnum.Published;

            Raise(new EventPublishedDomainEvent(Id));

            return Result.Success();
        }

        public void Reschedule(DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            if (DateRange.StartsAtUtc == startsAtUtc
                && DateRange.EndsAtUtc == endsAtUtc) return;

            DateRange = (startsAtUtc, endsAtUtc);

            Raise(new EventRescheduleDomainEvent(Id, DateRange.StartsAtUtc, DateRange.EndsAtUtc));
        }

        public Result Cancel(DateTime utcNow)
        {
            if (Status == EventStatusEnum.Cancelled)
                return Result.Failure(EventErrors.AlreadyCanceled);

            if (DateRange.StartsAtUtc < utcNow)
                return Result.Failure(EventErrors.AlreadyStarted);

            Status = EventStatusEnum.Cancelled;

            Raise(new EventCancelledDomainEvent(Id));

            return Result.Success();
        }
    }
}