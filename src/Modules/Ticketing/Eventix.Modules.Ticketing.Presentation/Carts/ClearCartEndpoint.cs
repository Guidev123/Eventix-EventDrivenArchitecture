using Eventix.Modules.Ticketing.Application.Carts.UseCases.Clear;
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
    internal sealed class ClearCartEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/v1/carts", async (ClaimsPrincipal claimsPrincipal, IMediatorHandler mediator) =>
            {
                var result = await mediator.DispatchAsync(new ClearCartCommand(claimsPrincipal.GetUserId())).ConfigureAwait(false);

                return result.Match(Results.NoContent, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.RemoveFromCart).WithTags(Tags.Carts);
        }
    }
}