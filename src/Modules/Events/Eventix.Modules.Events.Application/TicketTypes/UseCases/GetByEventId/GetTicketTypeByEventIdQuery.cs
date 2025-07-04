using Eventix.Modules.Events.Application.TicketTypes.Dtos;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetByEventId
{
    public sealed record GetTicketTypeByEventIdQuery(
        Guid EventId
        ) : IQuery<GetTicketTypeByEventIdResponse>;
}