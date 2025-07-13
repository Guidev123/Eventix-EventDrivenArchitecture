using Eventix.Modules.Ticketing.Application.Customers.UseCases.Update;
using Eventix.Modules.Users.IntegrationEvents.Users;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;

namespace Eventix.Modules.Ticketing.Infrastructure.Customers.IntegrationEvents
{
    internal sealed class UserUpdatedIntegrationEventHandler(IMediatorHandler mediator) : IntegrationEventHandler<UserUpdatedIntegrationEvent>
    {
        public override async Task ExecuteAsync(UserUpdatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var command = new UpdateCustomerCommand(
                integrationEvent.FirstName,
                integrationEvent.LastName
                );

            command.SetCustomerId(integrationEvent.UserId);

            var result = await mediator.DispatchAsync(command, cancellationToken).ConfigureAwait(false);

            if (result.IsFailure)
                throw new EventixException(nameof(UpdateCustomerCommand), result.Error);
        }
    }
}