using Eventix.Modules.Events.Application.TicketTypes.GetById;

namespace Eventix.Modules.Events.Application.TicketTypes.GetAll
{
    public record GetAllTicketTypesResponse(
        int Page,
        int PageSize,
        int TotalCount,
        IReadOnlyCollection<GetTicketTypeResponse> TicketsType
        );
}