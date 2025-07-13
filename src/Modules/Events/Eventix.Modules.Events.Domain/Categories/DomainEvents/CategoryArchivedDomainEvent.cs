using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Events.Domain.Categories.DomainEvents
{
    public record CategoryArchivedDomainEvent(Guid CategoryId) : DomainEvent(CategoryId);
}