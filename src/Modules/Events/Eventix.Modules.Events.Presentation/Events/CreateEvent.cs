using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Presentation.Events
{
    public static class CreateEvent
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("events", async (Request request) =>
            {
            }).WithTags(Tags.Events);
        }
    }

    public sealed record Request(
        string Title,
        string Description,
        string Location,
        DateTime StartsAtUtc,
        DateTime? EndsAtUtc
        );
}