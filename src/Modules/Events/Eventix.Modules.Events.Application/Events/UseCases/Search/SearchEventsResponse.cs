using Eventix.Modules.Events.Application.Events.UseCases.Get;

namespace Eventix.Modules.Events.Application.Events.UseCases.Search
{
    public record SearchEventsResponse(
        int Page,
        int PageSize,
        int TotalCount,
        IReadOnlyCollection<GetEventResponse> Events);
}