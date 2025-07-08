using Eventix.Modules.Events.Application.Events.UseCases.AttachLocation;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal sealed class AttachLocationForEventEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1/events/{id:guid}/location", async (Guid id,
                AttachEventLocationCommand command,
                IMediator mediator) =>
            {
                command.SetEventId(id);
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return result.Match(Results.NoContent, ApiResults.Problem);
            }).WithTags(Tags.Events)
            .RequireAuthorization(PolicyExtensions.ModifyEvents);
        }
    }
}