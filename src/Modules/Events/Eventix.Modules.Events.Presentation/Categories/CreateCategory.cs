using Eventix.Modules.Events.Application.Categories.Create;
using Eventix.Modules.Events.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Categories
{
    internal static class CreateCategory
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/categories", async (CreateCategoryCommand command, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return result.Match(
                    success => Results.Created($"/categories/{success.CategoryId}", success),
                    failure => ApiResults.Problem(failure)
                );
            })
            .WithTags(Tags.Categories);
        }
    }
}