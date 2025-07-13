using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Shared.Domain.DomainEvents
{
    public interface IDomainEvent : IEvent
    {
        Guid AggregateId { get; }
    }
}