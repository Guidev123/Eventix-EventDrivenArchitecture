using Eventix.Modules.Events.Application.Events.Get;

namespace Eventix.Modules.Events.Application.Events.GetAll
{
    public record GetAllEventsResponse(
        int Page,
        int PageSize,
        int TotalCount,
        IReadOnlyCollection<GetEventResponse> Events);
}