namespace Eventix.Shared.Application.EventBus
{
    public abstract record IntegrationEvent : IIntegrationEvent
    {
        protected IntegrationEvent(Guid correlationId, DateTime occurredOnUtc)
        {
            CorrelationId = correlationId;
            OccurredOnUtc = occurredOnUtc;
        }

        public Guid CorrelationId { get; }
        public DateTime OccurredOnUtc { get; }
    }
}