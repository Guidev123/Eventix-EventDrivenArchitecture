using Eventix.Shared.Application.EventBus;

namespace Eventix.Modules.Events.IntegrationEvents.Events
{
    public sealed record EventRescheduledIntegrationEvent : IntegrationEvent
    {
        public EventRescheduledIntegrationEvent(
            Guid correlationId,
            DateTime occurredOnUtc,
            Guid eventId,
            DateTime startsAtUtc,
            DateTime? endsAtUtc
            ) : base(correlationId, occurredOnUtc)
        {
            EventId = eventId;
            StartsAtUtc = startsAtUtc;
            EndsAtUtc = endsAtUtc;
        }
        public Guid EventId { get; init; }
        public DateTime StartsAtUtc { get; }
        public DateTime? EndsAtUtc { get; }
    }
}