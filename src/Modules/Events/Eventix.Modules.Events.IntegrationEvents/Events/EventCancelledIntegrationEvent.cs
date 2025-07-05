using Eventix.Shared.Application.EventBus;

namespace Eventix.Modules.Events.IntegrationEvents.Events
{
    public sealed record EventCancelledIntegrationEvent : IntegrationEvent
    {
        public EventCancelledIntegrationEvent(Guid id, DateTime occurredOnUtc, Guid eventId) : base(id, occurredOnUtc)
        {
            EventId = eventId;
        }
        public Guid EventId { get; init; }
    }
}