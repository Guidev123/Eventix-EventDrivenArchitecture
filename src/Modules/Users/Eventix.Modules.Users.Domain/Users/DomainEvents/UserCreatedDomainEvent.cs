using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Users.Domain.Users.DomainEvents
{
    public record UserCreatedDomainEvent(Guid UserId) : DomainEvent;
}