using Eventix.Modules.Ticketing.Application.Abstractions.Authentication;
using Eventix.Modules.Ticketing.Application.Carts.UseCases.Get;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Ticketing.Presentation.Carts
{
    internal sealed class GetCart : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/carts", async (ICustomerContext customerContext, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetCartQuery(customerContext.CustomerId)).ConfigureAwait(false);

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.Carts);
        }
    }
}