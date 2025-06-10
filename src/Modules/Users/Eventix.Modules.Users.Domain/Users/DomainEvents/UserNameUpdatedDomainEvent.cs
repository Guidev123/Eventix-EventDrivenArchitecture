using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Users.Domain.Users.DomainEvents
{
    public record UserNameUpdatedDomainEvent(
        Guid UserId,
        string FirstName,
        string LastName
        ) : DomainEvent;
}