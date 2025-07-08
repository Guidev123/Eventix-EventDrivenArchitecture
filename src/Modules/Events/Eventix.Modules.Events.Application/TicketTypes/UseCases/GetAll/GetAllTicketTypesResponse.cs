using Eventix.Modules.Events.Application.TicketTypes.DTOs;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetAll
{
    public sealed record GetAllTicketTypesResponse(
        int Page,
        int PageSize,
        int TotalCount,
        IReadOnlyCollection<TicketTypeResponse> TicketTypes
        );
}