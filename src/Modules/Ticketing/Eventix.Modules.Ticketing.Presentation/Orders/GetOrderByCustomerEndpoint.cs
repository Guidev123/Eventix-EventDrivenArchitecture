using Eventix.Modules.Ticketing.Application.Orders.UseCases.GetByCustomer;
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
    internal sealed class GetOrderByCustomerEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/orders", async (ClaimsPrincipal claimsPrincipal, IMediatorHandler mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetOrdersByCustomerQuery(claimsPrincipal.GetUserId())).ConfigureAwait(false);

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetOrders).WithTags(Tags.Orders);
        }
    }
}