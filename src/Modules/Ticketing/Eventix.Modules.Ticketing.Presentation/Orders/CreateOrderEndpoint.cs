using Eventix.Modules.Ticketing.Application.Orders.UseCases.Create;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Infrastructure.Authentication;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace Eventix.Modules.Ticketing.Presentation.Orders
{
    internal sealed class CreateOrderEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/orders", async (ClaimsPrincipal claimsPrincipal, IMediatorHandler mediator) =>
            {
                var result = await mediator.DispatchAsync(new CreateOrderCommand(claimsPrincipal.GetUserId()));

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            })
            .RequireAuthorization(PolicyExtensions.CreateOrder)
            .WithTags(Tags.Orders);
        }
    }
}