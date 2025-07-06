using Eventix.Modules.Ticketing.Application.Orders.UseCases.Create;
using Eventix.Shared.Infrastructure.Authentication;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;
using System.Security.Claims;

namespace Eventix.Modules.Ticketing.Presentation.Orders
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/orders", async (ClaimsPrincipal claimsPrincipal, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new CreateOrderCommand(claimsPrincipal.GetUserId()));

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            })
            .RequireAuthorization(PolicyExtensions.CreateOrder)
            .WithTags(Tags.Orders);
        }
    }
}