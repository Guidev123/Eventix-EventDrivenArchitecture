using Eventix.Modules.Ticketing.Application.Tickets.Dtos;
using Eventix.Modules.Ticketing.Application.Tickets.Mappers;
using Eventix.Modules.Ticketing.Domain.Tickets.Errors;
using Eventix.Modules.Ticketing.Domain.Tickets.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetByCode
{
    internal sealed class GetTicketByCodeHandler(ITicketRepository ticketRepository) : IQueryHandler<GetTicketByCodeQuery, TicketDto>
    {
        public async Task<Result<TicketDto>> ExecuteAsync(GetTicketByCodeQuery request, CancellationToken cancellationToken = default)
        {
            var ticket = await ticketRepository.GetByCodeAsync(request.Code, cancellationToken);
            if (ticket is null)
                return Result.Failure<TicketDto>(TicketErrors.NotFound(request.Code));

            return Result.Success(ticket.MapFromTicket());
        }
    }
}