namespace Eventix.Shared.Infrastructure.Outbox.Models
{
    public sealed record OutboxMessageResponse(
        Guid Id,
        string Content
        );
}