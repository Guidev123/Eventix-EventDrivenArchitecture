using Eventix.Modules.Ticketing.Application.Customers.UseCases.Create;
using Eventix.Modules.Users.IntegrationEvents.Users;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;

namespace Eventix.Modules.Ticketing.Infrastructure.Customers.IntegrationEvents
{
    internal sealed class UserRegisteredIntegrationEventHandler(IMediatorHandler mediator) : IntegrationEventHandler<UserRegisteredIntegrationEvent>
    {
        public override async Task ExecuteAsync(UserRegisteredIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var result = await mediator.DispatchAsync(new CreateCustomerCommand(
                integrationEvent.UserId,
                integrationEvent.Email,
                integrationEvent.FirstName,
                integrationEvent.LastName), cancellationToken);

            if (result.IsFailure)
                throw new EventixException(nameof(CreateCustomerCommand), result.Error);
        }
    }
}