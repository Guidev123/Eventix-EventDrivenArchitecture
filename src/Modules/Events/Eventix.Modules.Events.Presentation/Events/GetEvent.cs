using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Presentation.Events
{
    public static class GetEvent
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("events/{id:guid}", async (Guid id) =>
            {
            }).WithTags(Tags.Events);
        }
    }

    public sealed record Response(
        Guid Id,
        string Title,
        string Description,
        string Location,
        DateTime StartsAtUtc,
        DateTime? EndsAtUtc,
        EventStatusEnum Status
        );
}