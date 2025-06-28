using Eventix.Modules.Events.Application.TicketTypes.UseCases.GetById;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.TicketTypes
{
    internal class GetTicketTypeById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/events/ticket-types/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetTicketTypeByIdQuery(id)).ConfigureAwait(false);
                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetTicketTypes)
            .WithTags(Tags.TicketTypes);
        }
    }
}