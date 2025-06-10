using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.UseCases.GetAll
{
    public record GetAllEventsQuery(int Page, int PageSize) : IQuery<GetAllEventsResponse>
    {
    }
}