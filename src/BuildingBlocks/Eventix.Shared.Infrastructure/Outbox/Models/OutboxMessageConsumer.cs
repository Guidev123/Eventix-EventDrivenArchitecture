namespace Eventix.Shared.Infrastructure.Outbox.Models
{
    public sealed class OutboxMessageConsumer
    {
        public OutboxMessageConsumer(Guid outboxMessageId, string name)
        {
            OutboxMessageId = outboxMessageId;
            Name = name;
        }

        public Guid OutboxMessageId { get; init; }
        public string Name { get; init; } = default!;
    }
}