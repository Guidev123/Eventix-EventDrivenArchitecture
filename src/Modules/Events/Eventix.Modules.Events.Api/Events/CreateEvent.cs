using Eventix.Modules.Events.Api.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Api.Events
{
    public static class CreateEvent
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("events", async (Request request, EventsDbContext context) =>
            {
                var @event = new Event(Guid.NewGuid(), request.Title, request.Description, request.Location, request.StartsAtUtc, request.EndsAtUtc);

                context.Events.Add(@event);
                await context.SaveChangesAsync();

                return Results.Ok(@event.Id);
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