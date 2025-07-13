using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Events.Domain.Events.DomainEvents
{
    public sealed record EventCreatedDomainEvent : DomainEvent
    {
        public EventCreatedDomainEvent(Guid eventId) : base(eventId)
        {
            EventId = eventId;
        }

        public Guid EventId { get; private set; }
    }
}