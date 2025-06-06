using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Domain.Categories.DomainEvents
{
    public record CategoryRenamedDomainEvent(Guid CategoryId, string Name) : DomainEvent
    {
    }
}