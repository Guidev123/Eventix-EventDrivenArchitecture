using Eventix.Modules.Events.Application.TicketTypes.GetAll;
using Eventix.Modules.Events.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.TicketTypes
{
    internal static class GetAllTicketTypes
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/events/ticket-types", async (IMediator mediator,
                                                           [FromQuery] int page = EventsEndpoints.DEFAULT_PAGE,
                                                           [FromQuery] int pageSize = EventsEndpoints.DEFAULT_PAGE_SIZE) =>
            {
                var result = await mediator.DispatchAsync(new GetAllTicketTypesQuery(page, pageSize)).ConfigureAwait(false);

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .WithTags(Tags.TicketTypes);
        }
    }
}