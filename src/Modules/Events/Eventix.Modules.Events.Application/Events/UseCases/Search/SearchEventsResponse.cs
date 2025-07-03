using Eventix.Modules.Events.Application.Events.UseCases.GetById;

namespace Eventix.Modules.Events.Application.Events.UseCases.Search
{
    public sealed record SearchEventsResponse(
        int Page,
        int PageSize,
        int TotalCount,
        IReadOnlyCollection<GetEventResponse> Events);
}