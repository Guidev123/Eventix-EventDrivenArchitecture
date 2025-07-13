using Eventix.Modules.Users.Domain.Users.DomainEvents;
using Eventix.Modules.Users.IntegrationEvents.Users;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Users.Application.Users.DomainEvents
{
    internal sealed class UserNameUpdatedDomainEventHandler(IEventBus eventBus) : DomainEventHandler<UserNameUpdatedDomainEvent>
    {
        public override async Task ExecuteAsync(UserNameUpdatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await eventBus.PublishAsync(new UserUpdatedIntegrationEvent(
                domainEvent.CorrelationId,
                domainEvent.OccurredOnUtc,
                domainEvent.UserId,
                domainEvent.FirstName,
                domainEvent.LastName
                ), cancellationToken);
        }
    }
}