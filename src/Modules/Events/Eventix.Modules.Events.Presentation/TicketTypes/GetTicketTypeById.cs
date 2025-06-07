using Eventix.Modules.Events.Application.TicketTypes.GetById;
using Eventix.Modules.Events.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.TicketTypes
{
    internal static class GetTicketTypeById
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/events/ticket-types/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetTicketTypeByIdQuery(id)).ConfigureAwait(false);
                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .WithTags(Tags.TicketTypes);
        }
    }
}