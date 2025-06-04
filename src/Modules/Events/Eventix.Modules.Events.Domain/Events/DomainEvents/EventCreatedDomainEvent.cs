using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Domain.Events.DomainEvents
{
    public sealed record EventCreatedDomainEvent : DomainEvent
    {
        public EventCreatedDomainEvent(Guid eventId)
        {
            EventId = eventId;
        }

        public Guid EventId { get; private set; }
    }
}