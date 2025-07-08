using Eventix.Modules.Events.Application.TicketTypes.DTOs;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetByEventId
{
    public sealed record GetTicketTypeByEventIdResponse(
        IReadOnlyCollection<TicketTypeResponse> TicketTypes
        );
}