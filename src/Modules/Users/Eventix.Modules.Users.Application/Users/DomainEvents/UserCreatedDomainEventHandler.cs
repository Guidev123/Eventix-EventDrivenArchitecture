using Eventix.Modules.Users.Application.Users.UseCases.GetById;
using Eventix.Modules.Users.Domain.Users.DomainEvents;
using Eventix.Modules.Users.IntegrationEvents.Users;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Users.Application.Users.DomainEvents
{
    internal sealed class UserCreatedDomainEventHandler(IBus eventBus, IMediatorHandler mediator) : DomainEventHandler<UserCreatedDomainEvent>
    {
        public override async Task ExecuteAsync(UserCreatedDomainEvent notification, CancellationToken cancellationToken = default)
        {
            var userResult = await mediator.DispatchAsync(new GetUserByIdQuery(notification.UserId), cancellationToken);

            if (userResult.IsFailure)
                throw new EventixException(nameof(GetUserByIdQuery), userResult.Error);

            await eventBus.PublishAsync(new UserRegisteredIntegrationEvent(
                notification.CorrelationId,
                notification.OccurredOnUtc,
                userResult.Value.Id,
                userResult.Value.Email,
                userResult.Value.FirstName,
                userResult.Value.LastName
                ), cancellationToken);
        }
    }
}