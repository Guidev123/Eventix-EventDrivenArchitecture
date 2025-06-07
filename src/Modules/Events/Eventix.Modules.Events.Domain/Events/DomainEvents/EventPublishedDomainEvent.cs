using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Events.Domain.Events.DomainEvents
{
    public record EventPublishedDomainEvent : DomainEvent
    {
        public Guid EventId { get; }

        public EventPublishedDomainEvent(Guid eventId) => EventId = eventId;
    }
}