using Eventix.Modules.Users.Application.Users.UseCases.GetById;
using Eventix.Modules.Users.Domain.Users.DomainEvents;
using Eventix.Modules.Users.IntegrationEvents.Users;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;
using Eventix.Shared.Application.Messaging;
using MidR.Interfaces;

namespace Eventix.Modules.Users.Application.Users.DomainEvents
{
    internal sealed class UserCreatedDomainEventHandler(IEventBus eventBus, IMediator mediator) : DomainEventHandler<UserCreatedDomainEvent>
    {
        public override async Task ExecuteAsync(UserCreatedDomainEvent notification, CancellationToken cancellationToken = default)
        {
            var userResult = await mediator.DispatchAsync(new GetUserByIdQuery(notification.UserId), cancellationToken);

            if (userResult.IsFailure)
                throw new EventixException(nameof(GetUserByIdQuery), userResult.Error);

            await eventBus.PublishAsync(new UserRegisteredIntegrationEvent(
                notification.Id,
                notification.OccurredOnUtc,
                userResult.Value.Id,
                userResult.Value.Email,
                userResult.Value.FirstName,
                userResult.Value.LastName
                ), cancellationToken);
        }
    }
}