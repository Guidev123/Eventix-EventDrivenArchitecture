using Eventix.Modules.Events.Application.Events.Get;
using Eventix.Modules.Events.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal static class GetEventById
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/events/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var response = await mediator.DispatchAsync(new GetEventByIdQuery(id)).ConfigureAwait(false);

                return (await mediator
                .DispatchAsync(new GetEventByIdQuery(id))
                .ConfigureAwait(false))
                .Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.Events);
        }
    }
}