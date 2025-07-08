using Eventix.Modules.Ticketing.Application.Orders.UseCases.GetById;
using Eventix.Modules.Ticketing.Domain.Orders.DomainEvents;
using Eventix.Modules.Ticketing.IntegrationEvents.Orders;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;
using Eventix.Shared.Application.Messaging;
using MidR.Interfaces;

namespace Eventix.Modules.Ticketing.Application.Orders.DomainEvents
{
    internal sealed class OrderCreatedDomainEventHandler(IEventBus eventBus, IMediator mediator) : DomainEventHandler<OrderCreatedDomainEvent>
    {
        public override async Task ExecuteAsync(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var result = await mediator.DispatchAsync(new GetOrderByIdQuery(domainEvent.Id), cancellationToken);
            if (result.IsFailure)
                throw new EventixException(nameof(GetOrderByIdQuery), result.Error);

            var order = result.Value;

            await eventBus.PublishAsync(new OrderCreatedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                order.OrderId,
                order.TotalPrice!.Value,
                order.CustomerId,
                order.CreatedAtUtc,
                order.OrderItems.Select(x => new OrderCreatedIntegrationEvent.OrderItemDto(
                    x.OrderItemId,
                    x.OrderId,
                    x.TicketTypeId,
                    x.Quantity,
                    x.UnitPrice,
                    x.Price,
                    x.Currency)
                ).ToList()
                ), cancellationToken);
        }
    }
}