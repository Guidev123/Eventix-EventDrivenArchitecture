using Eventix.Modules.Ticketing.Application.Carts.UseCases.Get;
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
    internal sealed class GetCartEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/carts", async (ClaimsPrincipal claimsPrincipal, IMediatorHandler mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetCartQuery(claimsPrincipal.GetUserId())).ConfigureAwait(false);

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetCart).WithTags(Tags.Carts);
        }
    }
}