using Eventix.Modules.Events.Application.Categories.UseCases.Archive;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Categories
{
    internal sealed class ArchiveCategoryEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1/categories/{id:guid}/archive", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new ArchiveCategoryCommand(id)).ConfigureAwait(false);

                return result.Match(Results.NoContent, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.ModifyCategories)
            .WithTags(Tags.Categories);
        }
    }
}