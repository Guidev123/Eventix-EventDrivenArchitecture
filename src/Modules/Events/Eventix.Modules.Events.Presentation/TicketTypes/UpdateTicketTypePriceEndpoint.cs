using Eventix.Modules.Events.Application.TicketTypes.UseCases.UpdatePrice;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Presentation.TicketTypes
{
    internal sealed class UpdateTicketTypePriceEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1/events/ticket-types/{id:guid}/price", async (Guid id, UpdateTicketTypePriceCommand command, IMediatorHandler mediator) =>
            {
                command.SetTicketTypeId(id);
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return result.Match(Results.NoContent, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.ModifyTicketTypes)
            .WithTags(Tags.TicketTypes);
        }
    }
}