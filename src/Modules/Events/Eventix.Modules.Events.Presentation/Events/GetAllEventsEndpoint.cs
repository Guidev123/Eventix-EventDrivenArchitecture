using Eventix.Modules.Events.Application.Events.UseCases.GetAll;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal sealed class GetAllEventsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/events", async (IMediator mediator,
                                               [FromQuery] int page = PresentationModule.DEFAULT_PAGE,
                                               [FromQuery] int pageSize = PresentationModule.DEFAULT_PAGE_SIZE) =>
            {
                var result = await mediator.DispatchAsync(new GetAllEventsQuery(page, pageSize)).ConfigureAwait(false);

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetEvents)
            .WithTags(Tags.Events);
        }
    }
}