using Eventix.Modules.Events.Application.Abstractions.Messaging;

namespace Eventix.Modules.Events.Application.TicketTypes.GetAll
{
    public record GetAllTicketTypesQuery(int Page, int PageSize) : IQuery<List<GetAllTicketTypesResponse>>;
}