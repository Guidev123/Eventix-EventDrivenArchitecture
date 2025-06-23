using Eventix.Modules.Ticketing.Application.Abstractions.Authentication;
using Eventix.Modules.Ticketing.Application.Carts.UseCases.RemoveItem;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Ticketing.Presentation.Carts
{
    internal sealed class RemoveCartItem : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/v1/carts/{ticketTypeId:guid}", async (Guid ticketTypeId, ICustomerContext customerContext, IMediator mediator) =>
            {
                var command = new RemoveItemCommand(ticketTypeId);
                command.SetCustomerId(customerContext.CustomerId);

                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                result.Match(Results.NoContent, ApiResults.Problem);
            }).RequireAuthorization().WithTags(Tags.Carts);
        }
    }
}