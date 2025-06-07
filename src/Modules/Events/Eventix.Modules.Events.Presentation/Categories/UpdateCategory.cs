using Eventix.Modules.Events.Application.Categories.Update;
using Eventix.Modules.Events.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Categories
{
    internal static class UpdateCategory
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("api/v1/categories/{id:guid}", async (Guid id, UpdateCategoryCommand command, IMediator mediator) =>
            {
                command.SetCategoryId(id);
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return result.Match(Results.NoContent, ApiResults.Problem);
            })
            .WithTags(Tags.Categories);
        }
    }
}