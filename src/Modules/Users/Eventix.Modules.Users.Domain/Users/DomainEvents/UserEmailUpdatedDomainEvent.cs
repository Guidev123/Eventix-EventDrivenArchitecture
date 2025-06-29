using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Users.Domain.Users.DomainEvents
{
    public sealed record UserEmailUpdatedDomainEvent(
        Guid UserId,
        string Email
        ) : DomainEvent;
}