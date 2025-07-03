namespace Eventix.Shared.Infrastructure.Inbox.Models
{
    public sealed record InboxMessageResponse(
        Guid Id,
        string Content
        );
}