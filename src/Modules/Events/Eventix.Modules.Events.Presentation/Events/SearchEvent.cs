using Eventix.Modules.Events.Application.Events.UseCases.Search;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal class SearchEvent : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/events/search", async (
                IMediator mediator,
                [FromQuery] Guid? categoryId,
                [FromQuery] DateTime? startDate,
                [FromQuery] DateTime? endDate,
                [FromQuery] int page = PresentationModule.DEFAULT_PAGE,
                [FromQuery] int pageSize = PresentationModule.DEFAULT_PAGE_SIZE) =>
            {
                return (await mediator.DispatchAsync(
                    new SearchEventsQuery(categoryId, startDate,
                                          endDate, page, pageSize)).ConfigureAwait(false))
                                                                   .Match(Results.Ok, ApiResults.Problem);
            })
            .WithTags(Tags.Events);
        }
    }
}