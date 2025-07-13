using Eventix.Modules.Events.Application.Events.UseCases.Reschedule;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal sealed class RescheduleEventEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1/events/{id:guid}/reschedule", async (Guid id, RescheduleEventCommand command, IMediatorHandler mediator) =>
            {
                command.SetEventId(id);
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return result.Match(Results.NoContent, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.ModifyEvents)
            .WithTags(Tags.Events);
        }
    }
}