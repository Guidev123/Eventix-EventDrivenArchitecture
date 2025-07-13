using Eventix.Modules.Ticketing.Application.Carts.UseCases.AddItem;
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
    internal sealed class AddCartItemEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/carts/item", async (ClaimsPrincipal claimsPrincipal, AddItemToCartCommand command, IMediatorHandler mediator) =>
            {
                command.SetCustomerId(claimsPrincipal.GetUserId());
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.AddToCart).WithTags(Tags.Carts);
        }
    }
}