using Eventix.Modules.Ticketing.Application.Abstractions.Authentication;
using Eventix.Modules.Ticketing.Application.Orders.UseCases.GetByCustomer;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Ticketing.Presentation.Orders
{
    internal sealed class GetOrderByCustomer : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/orders", async (ICustomerContext customer, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetOrdersByCustomerQuery(customer.CustomerId)).ConfigureAwait(false);

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetOrders).WithTags(Tags.Orders);
        }
    }
}