using Eventix.Modules.Ticketing.Application.Tickets.DTOs;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetById
{
    public sealed record GetTicketByIdQuery(Guid TicketId) : IQuery<TicketResponse>;
}