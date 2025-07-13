using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Events.Domain.Events.DomainEvents
{
    public record EventCancelledDomainEvent : DomainEvent
    {
        public Guid EventId { get; }

        public EventCancelledDomainEvent(Guid eventId) : base(eventId)
        {
            EventId = eventId;
        }
    }
}