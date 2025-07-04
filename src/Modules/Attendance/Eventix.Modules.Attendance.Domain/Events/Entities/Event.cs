using Eventix.Modules.Attendance.Domain.Events.Errors;
using Eventix.Modules.Attendance.Domain.Events.ValueObjects;
using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Attendance.Domain.Events.Entities
{
    public sealed class Event : Entity, IAggregateRoot
    {
        private Event(Guid id, string title, string description, DateTime startsAtUtc, Location? location, DateTime? endsAtUtc = null)
        {
            Id = id;
            Specification = (title, description);
            Location = location;
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
            Location? location = null,
            DateTime? endsAtUtc = null)
        {
            var @event = new Event(id, title, description, startsAtUtc, location, endsAtUtc);

            return @event;
        }

        public void Reschedule(DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            if (DateRange.StartsAtUtc == startsAtUtc
                && DateRange.EndsAtUtc == endsAtUtc) return;

            DateRange = (startsAtUtc, endsAtUtc);
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotNull(Specification, EventErrors.SpecificationIsRequired.Description);
            AssertionConcern.EnsureNotNull(DateRange, EventErrors.DateRangeIsRequired.Description);
        }
    }
}