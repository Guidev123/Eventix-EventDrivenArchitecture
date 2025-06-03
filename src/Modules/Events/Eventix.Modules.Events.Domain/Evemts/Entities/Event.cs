using Eventix.Modules.Events.Domain.Evemts.Enumerators;
using Eventix.Modules.Events.Domain.Evemts.ValueObjects;

namespace Eventix.Modules.Events.Domain.Evemts.Entities
{
    public sealed class Event
    {
        public Event(string title, string description, DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            Id = Guid.NewGuid();
            Specification = (title, description);
            DateRange = (startsAtUtc, endsAtUtc);
            Status = EventStatusEnum.Draft;
        }

        private Event()
        { }

        public Guid Id { get; private set; }
        public EventSpecification Specification { get; private set; } = default!;
        public Location? Location { get; private set; } = default!;
        public DateRange DateRange { get; private set; } = default!;
        public EventStatusEnum Status { get; private set; }
    }
}