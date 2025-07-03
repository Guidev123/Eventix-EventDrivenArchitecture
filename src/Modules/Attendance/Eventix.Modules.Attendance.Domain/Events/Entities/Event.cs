using Eventix.Modules.Attendance.Domain.Events.DomainEvents;
using Eventix.Modules.Attendance.Domain.Events.Errors;
using Eventix.Modules.Attendance.Domain.Events.ValueObjects;
using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Attendance.Domain.Events.Entities
{
    public sealed class Event : Entity, IAggregateRoot
    {
        private Event(Guid id, string title, string description, DateTime startsAtUtc, DateTime? endsAtUtc = null)
        {
            Id = id;
            Specification = (title, description);
            DateRange = (startsAtUtc, endsAtUtc);
            Validate();
        }

        private Event()
        { }

        public EventSpecification Specification { get; private set; } = default!;

        public Location? Location { get; private set; }

        public DateRange DateRange { get; private set; } = default!;

        public static Event Create(
            Guid id,
            string title,
            string description,
            DateTime startsAtUtc,
            DateTime? endsAtUtc)
        {
            var @event = new Event(id, title, description, startsAtUtc, endsAtUtc);

            @event.Raise(new EventCreatedDomainEvent(
                @event.Id,
                @event.Specification.Title,
                @event.Specification.Description,
                @event.DateRange.StartsAtUtc,
                @event.DateRange.EndsAtUtc));

            return @event;
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotNull(Specification, EventErrors.SpecificationIsRequired.Description);
            AssertionConcern.EnsureNotNull(DateRange, EventErrors.DateRangeIsRequired.Description);
        }
    }
}