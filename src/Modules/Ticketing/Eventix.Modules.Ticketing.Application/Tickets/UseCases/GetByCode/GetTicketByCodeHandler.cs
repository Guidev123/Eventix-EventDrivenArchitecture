using Eventix.Modules.Ticketing.Application.Tickets.DTOs;
using Eventix.Modules.Ticketing.Application.Tickets.Mappers;
using Eventix.Modules.Ticketing.Domain.Tickets.Errors;
using Eventix.Modules.Ticketing.Domain.Tickets.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetByCode
{
    internal sealed class GetTicketByCodeHandler(ITicketRepository ticketRepository) : IQueryHandler<GetTicketByCodeQuery, TicketResponse>
    {
        public async Task<Result<TicketResponse>> ExecuteAsync(GetTicketByCodeQuery request, CancellationToken cancellationToken = default)
        {
            var ticket = await ticketRepository.GetByCodeAsync(request.Code, cancellationToken);
            if (ticket is null)
                return Result.Failure<TicketResponse>(TicketErrors.NotFound(request.Code));

            return Result.Success(ticket.MapFromTicket());
        }
    }
}