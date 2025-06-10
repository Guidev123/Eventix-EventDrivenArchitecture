using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Users.Domain.Users.DomainEvents
{
    public record UserEmailUpdatedDomainEvent(Guid UserId, string Email) : DomainEvent;
}