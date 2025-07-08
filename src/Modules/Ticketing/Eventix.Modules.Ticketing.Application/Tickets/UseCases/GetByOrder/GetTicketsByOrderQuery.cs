using Eventix.Modules.Ticketing.Application.Tickets.DTOs;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetByOrder
{
    public sealed record GetTicketsByOrderQuery(Guid OrderId) : IQuery<List<TicketResponse>>;
}