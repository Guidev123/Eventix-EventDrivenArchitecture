using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.Domain.Events.Interfaces;
using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.Errors;
using Eventix.Modules.Ticketing.Domain.Orders.Interfaces;
using Eventix.Modules.Ticketing.Domain.Tickets.Entities;
using Eventix.Modules.Ticketing.Domain.Tickets.Errors;
using Eventix.Modules.Ticketing.Domain.Tickets.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.CrateTicketBatch
{
    internal sealed class CreateTicketBatchHandler(IOrderRepository orderRepository,
                                                   ITicketRepository ticketRepository,
                                                   ITicketTypeRepository ticketTypeRepository) : ICommandHandler<CreateTicketBatchCommand>
    {
        public async Task<Result> ExecuteAsync(CreateTicketBatchCommand request, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order is null)
                return Result.Failure(OrderErrors.NotFound(request.OrderId));

            var orderResult = order.IssueTickets();
            if (orderResult.Error is not null
                && orderResult.IsFailure) return Result.Failure(orderResult.Error);

            var ticketsResult = await GetTicketsAsync([], order, cancellationToken);
            if (ticketsResult.Error is not null
                && ticketsResult.IsFailure) return Result.Failure(ticketsResult.Error);

            ticketRepository.InsertRange(ticketsResult.Value);

            var saveChanges = await ticketRepository.UnitOfWork.CommitAsync(cancellationToken);

            return saveChanges
                ? Result.Success()
                : Result.Failure(TicketErrors.FailToCreateTickets);
        }

        private async Task<Result<List<Ticket>>> GetTicketsAsync(List<Ticket> tickets, Order order, CancellationToken cancellationToken = default)
        {
            foreach (var orderItem in order.OrderItems)
            {
                var ticketType = await ticketTypeRepository.GetByIdAsync(orderItem.TicketTypeId, cancellationToken);
                if (ticketType is null)
                    return Result.Failure<List<Ticket>>(TicketTypeErrors.NotFound(orderItem.TicketTypeId));

                for (var iterator = 0; iterator < orderItem.Quantity.Value; iterator++)
                {
                    var ticket = Ticket.Create(order, ticketType);
                    tickets.Add(ticket);
                }
            }

            return tickets;
        }
    }
}