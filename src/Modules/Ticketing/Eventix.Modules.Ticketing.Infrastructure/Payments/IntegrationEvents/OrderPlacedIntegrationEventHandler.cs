using Eventix.Modules.Ticketing.Application.Payments.UseCases.PayOrder;
using Eventix.Modules.Ticketing.IntegrationEvents.Payments;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;

namespace Eventix.Modules.Ticketing.Infrastructure.Payments.IntegrationEvents
{
    internal sealed class OrderPlacedIntegrationEventHandler(IMediatorHandler mediator) : IntegrationEventHandler<OrderPlacedIntegrationEvent>
    {
        public override async Task ExecuteAsync(OrderPlacedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var result = await mediator.DispatchAsync(new PayOrderCommand(
                integrationEvent.OrderId,
                integrationEvent.TotalPrice,
                integrationEvent.Currency
                ), cancellationToken);

            if (result.IsFailure)
                throw new EventixException(nameof(PayOrderCommand), result.Error);
        }
    }
}