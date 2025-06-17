using Eventix.Modules.Ticketing.Application.Customers.UseCases.Create;
using Eventix.Modules.Users.IntegrationEvents.Users;
using Eventix.Shared.Application.Exceptions;
using MassTransit;
using MidR.Interfaces;

namespace Eventix.Modules.Ticketing.Infrastructure.Customers.Consumers
{
    public sealed class UserRegisteredIntegrationEventConsumer(IMediator mediator) : IConsumer<UserRegisteredIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
        {
            var result = await mediator.DispatchAsync(new CreateCustomerCommand(
                context.Message.UserId,
                context.Message.Email,
                context.Message.FirstName,
                context.Message.LastName));

            if (result.IsFailure)
                throw new EventixException(nameof(CreateCustomerCommand), result.Error);
        }
    }
}