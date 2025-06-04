using Eventix.Modules.Events.Application.Events.Create;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal static class CreateEvent
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("events", async (CreateEventCommand command, IMediator mediator) =>
            {
                var response = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return response.Id == Guid.Empty
                ? Results.BadRequest()
                : Results.Created($"/events/{response.Id}", response);
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