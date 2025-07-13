using Eventix.Modules.Events.Application.Events.UseCases.Create;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal sealed class CreateEventEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/events", async (CreateEventCommand command, IMediatorHandler mediator) =>
            {
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return result.Match(
                    success => Results.Created($"/events/{result.Value.Id}", result.Value),
                    failure => ApiResults.Problem(failure)
                );
            }).RequireAuthorization(PolicyExtensions.ModifyEvents).WithTags(Tags.Events);
        }
    }
}