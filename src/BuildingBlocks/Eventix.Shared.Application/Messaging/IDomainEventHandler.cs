using Eventix.Shared.Domain.DomainEvents;
using MidR.Interfaces;

namespace Eventix.Shared.Application.Messaging
{
    public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent;
}