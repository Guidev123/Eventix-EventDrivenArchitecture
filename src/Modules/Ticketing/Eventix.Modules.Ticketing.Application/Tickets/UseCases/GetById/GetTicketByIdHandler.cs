using Eventix.Modules.Ticketing.Application.Tickets.Dtos;
using Eventix.Modules.Ticketing.Application.Tickets.Mappers;
using Eventix.Modules.Ticketing.Domain.Tickets.Errors;
using Eventix.Modules.Ticketing.Domain.Tickets.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetById
{
    internal sealed class GetTicketByIdHandler(ITicketRepository ticketRepository) : IQueryHandler<GetTicketByIdQuery, TicketDto>
    {
        public async Task<Result<TicketDto>> ExecuteAsync(GetTicketByIdQuery request, CancellationToken cancellationToken = default)
        {
            var ticket = await ticketRepository.GetByIdAsync(request.TicketId, cancellationToken);
            if (ticket is null)
                return Result.Failure<TicketDto>(TicketErrors.NotFound(request.TicketId));

            return Result.Success(ticket.MapFromTicket());
        }
    }
}