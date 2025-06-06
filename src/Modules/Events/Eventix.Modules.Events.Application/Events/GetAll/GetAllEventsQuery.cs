using Eventix.Modules.Events.Application.Abstractions.Messaging;

namespace Eventix.Modules.Events.Application.Events.GetAll
{
    public record GetAllEventsQuery(int Page, int PageSize) : IQuery<List<GetAllEventsResponse>>
    {
    }
}