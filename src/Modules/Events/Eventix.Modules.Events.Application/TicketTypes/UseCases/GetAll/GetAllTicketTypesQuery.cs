using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetAll
{
    public record GetAllTicketTypesQuery(int Page, int PageSize) : IQuery<GetAllTicketTypesResponse>;
}