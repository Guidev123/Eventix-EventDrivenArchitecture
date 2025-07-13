namespace Eventix.Shared.Domain.DomainEvents
{
    public abstract record DomainEvent : IDomainEvent
    {
        public DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
            CorrelationId = Guid.NewGuid();
            OccurredOnUtc = DateTime.UtcNow;
        }

        public DomainEvent(Guid correlationId, Guid aggregateId, DateTime occurredOnUtc)
        {
            CorrelationId = correlationId;
            OccurredOnUtc = occurredOnUtc;
            AggregateId = aggregateId;
        }

        public Guid CorrelationId { get; }

        public DateTime OccurredOnUtc { get; }

        public Guid AggregateId { get; }
    }
}