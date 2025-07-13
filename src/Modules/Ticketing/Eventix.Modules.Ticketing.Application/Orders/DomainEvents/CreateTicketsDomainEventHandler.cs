using Eventix.Modules.Ticketing.Application.Tickets.UseCases.CrateTicketBatch;
using Eventix.Modules.Ticketing.Domain.Orders.DomainEvents;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Application.Exceptions;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Orders.DomainEvents
{
    internal sealed class CreateTicketsDomainEventHandler(IMediatorHandler mediator) : DomainEventHandler<OrderCreatedDomainEvent>
    {
        public override async Task ExecuteAsync(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var result = await mediator.DispatchAsync(new CreateTicketBatchCommand(domainEvent.OrderId), cancellationToken);
            if (result.IsFailure)
                throw new EventixException(nameof(CreateTicketBatchCommand), result.Error);
        }
    }
}