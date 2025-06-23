using Eventix.Modules.Events.Application.TicketTypes.UseCases.UpdatePrice;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.TicketTypes
{
    internal class UpdateTicketTypePrice : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1/events/ticket-types/{id:guid}/price", async (Guid id, UpdateTicketTypePriceCommand command, IMediator mediator) =>
            {
                command.SetTicketTypeId(id);
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);
                return result.Match(Results.NoContent, ApiResults.Problem);
            }).RequireAuthorization()
            .WithTags(Tags.TicketTypes);
        }
    }
}