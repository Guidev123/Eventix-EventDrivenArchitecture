using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.UseCases.Search
{
    public sealed record SearchEventsQuery(
        Guid? CategoryId,
        DateTime? StartDate,
        DateTime? EndDate,
        int Page,
        int PageSize)
        : IQuery<SearchEventsResponse>;
}