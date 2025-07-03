namespace Eventix.Shared.Infrastructure.Inbox.Models
{
    public sealed class InboxMessage
    {
        public Guid Id { get; init; }
        public string Type { get; init; } = null!;
        public string Content { get; init; } = null!;
        public DateTime OccurredOnUtc { get; init; }
        public DateTime? ProcessedOnUtc { get; init; }
        public string? Error { get; init; }
    }
}