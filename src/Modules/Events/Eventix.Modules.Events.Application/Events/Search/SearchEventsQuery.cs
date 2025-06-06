using Eventix.Modules.Events.Application.Abstractions.Messaging;

namespace Eventix.Modules.Events.Application.Events.Search
{
    public record SearchEventsQuery(
        Guid? CategoryId,
        DateTime? StartDate,
        DateTime? EndDate,
        int Page,
        int PageSize)
        : IQuery<SearchEventsResponse>;
}