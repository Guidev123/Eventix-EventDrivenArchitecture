using Eventix.Shared.Application.EventBus;

namespace Eventix.Modules.Events.IntegrationEvents.Events
{
    public sealed record EventCancelledIntegrationEvent : IntegrationEvent
    {
        public EventCancelledIntegrationEvent(Guid correlationId, DateTime occurredOnUtc, Guid eventId) : base(correlationId, occurredOnUtc)
        {
            EventId = eventId;
        }
        public Guid EventId { get; init; }
    }
}