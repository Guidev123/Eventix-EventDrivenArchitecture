using Eventix.Modules.Ticketing.Application.Carts.UseCases.RemoveItem;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Infrastructure.Authentication;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace Eventix.Modules.Ticketing.Presentation.Carts
{
    internal sealed class RemoveCartItemEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/v1/carts/ticket-types/{id:guid}", async (Guid id, ClaimsPrincipal claimsPrincipal, IMediatorHandler mediator) =>
            {
                var command = new RemoveItemCommand(id);
                command.SetCustomerId(claimsPrincipal.GetUserId());

                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                result.Match(Results.NoContent, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.RemoveFromCart).WithTags(Tags.Carts);
        }
    }
}