using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Events.Domain.Categories.DomainEvents
{
    public record CategoryRenamedDomainEvent(Guid CategoryId, string Name) : DomainEvent
    {
    }
}