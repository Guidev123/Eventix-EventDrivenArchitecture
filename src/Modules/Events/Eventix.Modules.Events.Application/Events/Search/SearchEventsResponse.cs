using Eventix.Modules.Events.Application.Events.Get;

namespace Eventix.Modules.Events.Application.Events.Search
{
    public record SearchEventsResponse(
        int Page,
        int PageSize,
        int TotalCount,
        IReadOnlyCollection<GetEventResponse> Events);
}