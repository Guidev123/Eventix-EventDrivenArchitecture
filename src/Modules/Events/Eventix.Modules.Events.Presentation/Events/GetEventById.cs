using Eventix.Modules.Events.Application.Events.UseCases.GetById;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.Events
{
    internal class GetEventById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/events/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                return (await mediator
                .DispatchAsync(new GetEventByIdQuery(id))
                .ConfigureAwait(false))
                .Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization().WithTags(Tags.Events);
        }
    }
}