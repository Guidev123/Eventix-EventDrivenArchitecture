using Eventix.Modules.Events.Application.TicketTypes.UseCases.GetById;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetAll
{
    public record GetAllTicketTypesResponse(
        int Page,
        int PageSize,
        int TotalCount,
        IReadOnlyCollection<GetTicketTypeResponse> TicketsType
        );
}