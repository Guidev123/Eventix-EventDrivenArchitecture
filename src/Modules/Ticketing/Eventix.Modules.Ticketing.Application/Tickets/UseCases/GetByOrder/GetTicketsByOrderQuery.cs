using Eventix.Modules.Ticketing.Application.Tickets.Dtos;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetByOrder
{
    public record GetTicketsByOrderQuery(Guid OrderId) : IQuery<List<TicketDto>>;
}