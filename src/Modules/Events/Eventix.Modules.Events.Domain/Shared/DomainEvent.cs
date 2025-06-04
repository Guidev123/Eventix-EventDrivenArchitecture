namespace Eventix.Modules.Events.Domain.Shared
{
    public abstract record DomainEvent : IDomainEvent
    {
        public DomainEvent()
        {
            Id = Guid.NewGuid();
            OccurredOnUtc = DateTime.UtcNow;
        }

        public DomainEvent(Guid id, DateTime occurredOnUtc)
        {
            Id = id;
            OccurredOnUtc = occurredOnUtc;
        }

        public Guid Id { get; }
        public DateTime OccurredOnUtc { get; }
    }
}