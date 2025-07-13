using Eventix.Modules.Events.Application.Categories.UseCases.Update;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Presentation.Categories
{
    internal sealed class UpdateCategoryEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("api/v1/categories/{id:guid}", async (Guid id, UpdateCategoryCommand command, IMediatorHandler mediator) =>
            {
                command.SetCategoryId(id);
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return result.Match(Results.NoContent, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.ModifyCategories)
            .WithTags(Tags.Categories);
        }
    }
}