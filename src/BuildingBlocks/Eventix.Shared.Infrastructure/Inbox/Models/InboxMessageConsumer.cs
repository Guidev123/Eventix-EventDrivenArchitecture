namespace Eventix.Shared.Infrastructure.Inbox.Models
{
    public sealed class InboxMessageConsumer
    {
        public InboxMessageConsumer(Guid inboxMessageId, string name)
        {
            InboxMessageId = inboxMessageId;
            Name = name;
        }

        public Guid InboxMessageId { get; init; }
        public string Name { get; init; } = default!;
    }
}