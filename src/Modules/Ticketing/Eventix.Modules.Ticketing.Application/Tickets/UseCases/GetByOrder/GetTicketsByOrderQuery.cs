using Eventix.Modules.Ticketing.Application.Tickets.Dtos;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetByOrder
{
    public sealed record GetTicketsByOrderQuery(Guid OrderId) : IQuery<List<TicketDto>>;
}