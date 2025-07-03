using Eventix.Modules.Ticketing.Application.Tickets.Dtos;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetByCode
{
    public sealed record GetTicketByCodeQuery(string Code) : IQuery<TicketDto>;
}