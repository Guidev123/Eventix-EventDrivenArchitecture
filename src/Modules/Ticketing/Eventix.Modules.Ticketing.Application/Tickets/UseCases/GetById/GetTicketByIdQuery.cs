using Eventix.Modules.Ticketing.Application.Tickets.Dtos;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetById
{
    public sealed record GetTicketByIdQuery(Guid TicketId) : IQuery<TicketDto>;
}