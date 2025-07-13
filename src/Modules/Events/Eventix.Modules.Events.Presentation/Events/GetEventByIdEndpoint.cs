using Eventix.Modules.Events.Application.Events.UseCases.GetById;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal sealed class GetEventByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/events/{id:guid}", async (Guid id, IMediatorHandler mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetEventByIdQuery(id)).ConfigureAwait(false);

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetEvents).WithTags(Tags.Events);
        }
    }
}