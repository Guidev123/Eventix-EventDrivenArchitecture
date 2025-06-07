using Eventix.Modules.Events.Application.TicketTypes.UpdatePrice;
using Eventix.Modules.Events.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.TicketTypes
{
    internal static class UpdateTicketTypePrice
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1/events/ticket-types/{id:guid}/price", async (Guid id, UpdateTicketTypePriceCommand command, IMediator mediator) =>
            {
                command.SetTicketTypeId(id);
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);
                return result.Match(Results.NoContent, ApiResults.Problem);
            })
            .WithTags(Tags.TicketTypes);
        }
    }
}