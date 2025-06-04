using Eventix.Modules.Events.Application.Events.Get;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal static class GetEvent
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("events/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var response = await mediator.DispatchAsync(new GetEventByIdQuery(id)).ConfigureAwait(false);

                return response is not null
                    ? Results.Ok(response)
                    : Results.NotFound();
            }).WithTags(Tags.Events);
        }
    }
}