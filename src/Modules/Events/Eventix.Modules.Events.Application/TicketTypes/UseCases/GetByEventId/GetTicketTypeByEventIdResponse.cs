using Eventix.Modules.Events.Application.TicketTypes.Dtos;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetByEventId
{
    public sealed record GetTicketTypeByEventIdResponse(
        IReadOnlyCollection<TicketTypeDto> TicketTypes
        );
}