using Eventix.Modules.Events.Application.Categories.GetById;
using Eventix.Modules.Events.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Categories
{
    internal static class GetCategoryById
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/categories/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetCategoryByIdQuery(id)).ConfigureAwait(false);

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .WithTags(Tags.Categories);
        }
    }
}