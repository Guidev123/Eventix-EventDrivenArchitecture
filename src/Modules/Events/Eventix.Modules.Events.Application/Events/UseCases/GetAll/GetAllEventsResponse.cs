using Eventix.Modules.Events.Application.Events.UseCases.GetById;

namespace Eventix.Modules.Events.Application.Events.UseCases.GetAll
{
    public sealed record GetAllEventsResponse(
        int Page,
        int PageSize,
        int TotalCount,
        IReadOnlyCollection<GetEventResponse> Events);
}