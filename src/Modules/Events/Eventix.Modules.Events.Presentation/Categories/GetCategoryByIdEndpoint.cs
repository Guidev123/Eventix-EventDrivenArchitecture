using Eventix.Modules.Events.Application.Categories.UseCases.GetById;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Presentation.Categories
{
    internal sealed class GetCategoryByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/categories/{id:guid}", async (Guid id, IMediatorHandler mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetCategoryByIdQuery(id)).ConfigureAwait(false);

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetCategories)
            .WithTags(Tags.Categories);
        }
    }
}