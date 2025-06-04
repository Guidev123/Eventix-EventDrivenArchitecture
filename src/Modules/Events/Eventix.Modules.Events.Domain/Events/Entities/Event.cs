using Eventix.Modules.Events.Domain.Events.DomainEvents;
using Eventix.Modules.Events.Domain.Events.Enumerators;
using Eventix.Modules.Events.Domain.Events.ValueObjects;
using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Domain.Events.Entities
{
    public sealed class Event : Entity
    {
        private Event(string title, string description, DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            Specification = (title, description);
            DateRange = (startsAtUtc, endsAtUtc);
            Status = EventStatusEnum.Draft;
        }

        private Event()
        { }

        public EventSpecification Specification { get; private set; } = default!;
        public Location? Location { get; private set; } = default!;
        public DateRange DateRange { get; private set; } = default!;
        public EventStatusEnum Status { get; private set; }

        public static Event Create(string title, string description, DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            var @event = new Event(title, description, startsAtUtc, endsAtUtc);

            @event.Raise(new EventCreatedDomainEvent(@event.Id));

            return @event;
        }
    }
}