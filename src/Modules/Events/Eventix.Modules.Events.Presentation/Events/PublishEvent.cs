using Eventix.Modules.Events.Application.Events.Publish;
using Eventix.Modules.Events.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal static class PublishEvent
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1/events/{id:guid}/publish", async (Guid id, IMediator mediator) =>
            {
                return (await mediator
                .DispatchAsync(new PublishEventCommand(id))
                .ConfigureAwait(false))
                .Match(Results.NoContent, ApiResults.Problem);
            })
            .WithTags(Tags.Events);
        }
    }
}