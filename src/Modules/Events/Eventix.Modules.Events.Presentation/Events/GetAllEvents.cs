using Eventix.Modules.Events.Application.Events.GetAll;
using Eventix.Modules.Events.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal sealed class GetAllEvents
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/events", async (IMediator mediator,
                                               [FromQuery] int page = EventsEndpoints.DEFAULT_PAGE,
                                               [FromQuery] int pageSize = EventsEndpoints.DEFAULT_PAGE_SIZE) =>
            {
                return (await mediator
                .DispatchAsync(new GetAllEventsQuery(page, pageSize))
                .ConfigureAwait(false))
                .Match(Results.Ok, ApiResults.Problem);
            })
            .WithTags(Tags.Events);
        }
    }
}