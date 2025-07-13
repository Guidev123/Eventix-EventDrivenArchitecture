using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Shared.Application.EventSourcing
{
    public interface IEventSourcingRepository
    {
        Task SaveAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent;

        Task<IEnumerable<StoredEvent>> GetAllAsync(Guid aggregateId);
    }
}