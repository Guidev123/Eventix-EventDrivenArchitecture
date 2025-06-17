namespace Eventix.Shared.Application.EventBus
{
    public abstract record IntegrationEvent : IIntegrationEvent
    {
        protected IntegrationEvent(Guid id, DateTime occurredOnUtc)
        {
            Id = id;
            OccurredOnUtc = occurredOnUtc;
        }

        public Guid Id { get; }
        public DateTime OccurredOnUtc { get; }
    }
}