using Eventix.Modules.Events.Api.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Events.Api.Events
{
    public static class GetEvent
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("events/{id:guid}", async (Guid id, EventsDbContext context) =>
            {
                var @event = await context.Events.Select(x =>
                new Response(x.Id, x.Title,
                    x.Description, x.Location,
                    x.StartsAtUtc, x.EndsAtUtc,
                    x.Status)).FirstOrDefaultAsync(x => x.Id == id);

                return @event is null ? Results.NotFound() : Results.Ok(@event);
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