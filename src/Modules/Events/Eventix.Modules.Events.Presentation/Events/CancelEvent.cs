using Eventix.Modules.Events.Application.Events.Cancel;
using Eventix.Modules.Events.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal static class CancelEvent
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/v1/events/{id:guid}/cancel", async (Guid id, IMediator mediator) =>
            {
                return (await mediator
                .DispatchAsync(new CancelEventCommand(id))
                .ConfigureAwait(false))
                .Match(Results.NoContent, ApiResults.Problem);
            }).WithTags(Tags.Events);
        }
    }
}