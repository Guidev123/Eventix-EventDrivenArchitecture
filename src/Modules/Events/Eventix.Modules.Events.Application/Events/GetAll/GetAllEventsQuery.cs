using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.GetAll
{
    public record GetAllEventsQuery(int Page, int PageSize) : IQuery<GetAllEventsResponse>
    {
    }
}