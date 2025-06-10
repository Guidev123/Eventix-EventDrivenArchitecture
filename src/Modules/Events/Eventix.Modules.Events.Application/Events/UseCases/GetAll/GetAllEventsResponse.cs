using Eventix.Modules.Events.Application.Events.UseCases.Get;

namespace Eventix.Modules.Events.Application.Events.UseCases.GetAll
{
    public record GetAllEventsResponse(
        int Page,
        int PageSize,
        int TotalCount,
        IReadOnlyCollection<GetEventResponse> Events);
}