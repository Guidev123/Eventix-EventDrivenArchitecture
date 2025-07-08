using Eventix.Modules.Ticketing.Application.Tickets.DTOs;
using Eventix.Modules.Ticketing.Application.Tickets.Mappers;
using Eventix.Modules.Ticketing.Domain.Tickets.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetByOrder
{
    internal sealed class GetTicketsByOrderHandler(ITicketRepository ticketRepository) : IQueryHandler<GetTicketsByOrderQuery, List<TicketResponse>>
    {
        public async Task<Result<List<TicketResponse>>> ExecuteAsync(GetTicketsByOrderQuery request, CancellationToken cancellationToken = default)
        {
            var tickets = await ticketRepository.GetByOrderId(request.OrderId, cancellationToken);

            return Result.Success(tickets.Select(t => t.MapFromTicket()).ToList());
        }
    }
}