using Eventix.Modules.Events.IntegrationEvents.TicketTypes;
using Eventix.Modules.Ticketing.Application.TicketTypes.UseCases.UpdatePrice;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;

namespace Eventix.Modules.Ticketing.Infrastructure.Events.IntegrationEvents
{
    internal sealed class TicketTypePriceChangedIntegrationEventHandler(IMediatorHandler mediator) : IntegrationEventHandler<TicketTypePriceChangedIntegrationEvent>
    {
        public override async Task ExecuteAsync(TicketTypePriceChangedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var result = await mediator.DispatchAsync(new UpdateTicketTypePriceCommand(
                integrationEvent.TicketTypeId,
                integrationEvent.Price
                ), cancellationToken);

            if (result.IsFailure)
                throw new EventixException(nameof(UpdateTicketTypePriceCommand), result.Error);
        }
    }
}