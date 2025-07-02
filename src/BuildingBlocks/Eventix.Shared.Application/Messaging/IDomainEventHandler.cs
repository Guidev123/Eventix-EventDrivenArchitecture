using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Shared.Application.Messaging
{
    public interface IDomainEventHandler<in TDomainEvent> : IDomainEventHandler
        where TDomainEvent : IDomainEvent
    {
        Task ExecuteAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }

    public interface IDomainEventHandler
    {
        Task ExecuteAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
}