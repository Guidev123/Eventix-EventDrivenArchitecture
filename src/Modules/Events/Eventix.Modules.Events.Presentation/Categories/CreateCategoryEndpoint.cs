using Eventix.Modules.Events.Application.Categories.UseCases.Create;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Categories
{
    internal sealed class CreateCategoryEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/categories", async (CreateCategoryCommand command, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return result.Match(
                    success => Results.Created($"/categories/{success.CategoryId}", success),
                    failure => ApiResults.Problem(failure)
                );
            }).RequireAuthorization(PolicyExtensions.ModifyCategories)
            .WithTags(Tags.Categories);
        }
    }
}