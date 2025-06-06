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
            app.MapPut("api/v1/events/{id:guid}/reschedule", async (Guid id, Request request, IMediator mediator) =>
            {
                return (await mediator.DispatchAsync(
                    new RescheduleEventCommand(id, request.StartsAtUtc, request.EndsAtUtc))
                .ConfigureAwait(false))
                .Match(Results.NoContent, ApiResults.Problem);
            })
            .WithTags(Tags.Events);
        }

        internal sealed record Request
        {
            public DateTime StartsAtUtc { get; init; }

            public DateTime? EndsAtUtc { get; init; }
        }
    }
}