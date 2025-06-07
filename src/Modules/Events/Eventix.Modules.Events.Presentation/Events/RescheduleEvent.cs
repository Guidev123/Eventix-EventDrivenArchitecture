using Eventix.Modules.Events.Application.Events.Reschedule;
using Eventix.Modules.Events.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal static class RescheduleEvent
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1/events/{id:guid}/reschedule", async (Guid id, RescheduleEventCommand command, IMediator mediator) =>
            {
                command.SetEventId(id);
                return (await mediator.DispatchAsync(command)
                .ConfigureAwait(false))
                .Match(Results.NoContent, ApiResults.Problem);
            })
            .WithTags(Tags.Events);
        }
    }
}