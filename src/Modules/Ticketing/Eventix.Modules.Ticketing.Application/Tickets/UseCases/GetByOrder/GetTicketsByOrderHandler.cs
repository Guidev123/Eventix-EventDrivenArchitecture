using Eventix.Modules.Ticketing.Application.Tickets.Dtos;
using Eventix.Modules.Ticketing.Application.Tickets.Mappers;
using Eventix.Modules.Ticketing.Domain.Tickets.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetByOrder
{
    internal sealed class GetTicketsByOrderHandler(ITicketRepository ticketRepository) : IQueryHandler<GetTicketsByOrderQuery, List<TicketDto>>
    {
        public async Task<Result<List<TicketDto>>> ExecuteAsync(GetTicketsByOrderQuery request, CancellationToken cancellationToken = default)
        {
            var tickets = await ticketRepository.GetByOrderId(request.OrderId, cancellationToken);

            return Result.Success(tickets.Select(t => t.MapFromTicket()).ToList());
        }
    }
}