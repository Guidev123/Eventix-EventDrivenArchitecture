using Eventix.Modules.Events.Application.TicketTypes.UseCases.GetAll;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.TicketTypes
{
    internal class GetAllTicketTypes : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/events/ticket-types", async (IMediator mediator,
                                                           [FromQuery] int page = PresentationModule.DEFAULT_PAGE,
                                                           [FromQuery] int pageSize = PresentationModule.DEFAULT_PAGE_SIZE) =>
            {
                var result = await mediator.DispatchAsync(new GetAllTicketTypesQuery(page, pageSize)).ConfigureAwait(false);

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization()
            .WithTags(Tags.TicketTypes);
        }
    }
}