using Eventix.Modules.Events.Application.Events.UseCases.Publish;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal sealed class PublishEventEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1/events/{id:guid}/publish", async (Guid id, IMediatorHandler mediator) =>
            {
                var result = await mediator.DispatchAsync(new PublishEventCommand(id)).ConfigureAwait(false);

                return result.Match(Results.NoContent, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetEvents)
            .WithTags(Tags.Events);
        }
    }
}