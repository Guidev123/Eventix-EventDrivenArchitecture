using Eventix.Modules.Events.Application.TicketTypes.DTOs;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetByEventId
{
    public sealed record GetTicketTypeByEventIdQuery(
        Guid EventId
        ) : IQuery<GetTicketTypeByEventIdResponse>;
}