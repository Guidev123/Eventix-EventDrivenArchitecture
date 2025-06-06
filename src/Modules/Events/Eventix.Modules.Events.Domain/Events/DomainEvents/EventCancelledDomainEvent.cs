using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Domain.Events.DomainEvents
{
    public record EventCancelledDomainEvent : DomainEvent
    {
        public Guid EventId { get; }

        public EventCancelledDomainEvent(Guid eventId)
        {
            EventId = eventId;
        }
    }
}