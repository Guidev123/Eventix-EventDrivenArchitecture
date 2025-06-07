using Eventix.Modules.Events.Application.Categories.Archive;
using Eventix.Modules.Events.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Categories
{
    internal static class ArchiveCategory
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1/categories/{id:guid}/archive", async (Guid id, IMediator mediator) =>
            {
                return (await mediator
                .DispatchAsync(new ArchiveCategoryCommand(id)))
                .Match(Results.NoContent, ApiResults.Problem);
            })
            .WithTags(Tags.Categories);
        }
    }
}