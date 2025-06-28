using Eventix.Modules.Events.Application.Events.UseCases.Cancel;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal class CancelEvent : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/v1/events/{id:guid}/cancel", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new CancelEventCommand(id)).ConfigureAwait(false);

                return result.Match(Results.NoContent, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.ModifyEvents)
            .WithTags(Tags.Events);
        }
    }
}