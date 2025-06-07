using Eventix.Modules.Events.Application.Categories.GetAll;
using Eventix.Modules.Events.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Categories
{
    internal static class GetAllCategories
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/categories", async (IMediator mediator,
                                                  [FromQuery] int page = EventsEndpoints.DEFAULT_PAGE,
                                                  [FromQuery] int pageSize = EventsEndpoints.DEFAULT_PAGE_SIZE) =>
            {
                var result = await mediator.DispatchAsync(new GetAllCategoriesQuery(page, pageSize)).ConfigureAwait(false);

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .WithTags(Tags.Categories);
        }
    }
}