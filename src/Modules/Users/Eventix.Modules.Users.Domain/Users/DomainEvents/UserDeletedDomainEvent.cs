using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Users.Domain.Users.DomainEvents
{
    public sealed record UserDeletedDomainEvent(
        Guid UserId
        ) : DomainEvent(UserId);
}