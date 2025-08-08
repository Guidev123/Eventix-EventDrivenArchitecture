using Eventix.Modules.Ticketing.Domain.Orders.DomainEvents;
using Eventix.Modules.Ticketing.IntegrationEvents.Payments;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Orders.DomainEvents
{
    internal sealed class OrderPlacedDomainEventHandler(IBus eventBus) : DomainEventHandler<OrderPlacedDomainEvent>
    {
        public override async Task ExecuteAsync(OrderPlacedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await eventBus.PublishAsync(
                new OrderPlacedIntegrationEvent(
                    domainEvent.CorrelationId,
                    domainEvent.OccurredOnUtc,
                    domainEvent.OrderId,
                    domainEvent.TotalPrice,
                    domainEvent.Currency
                ), cancellationToken);
        }
    }
}