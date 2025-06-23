using Eventix.Modules.Ticketing.Application.Abstractions.Authentication;
using Eventix.Modules.Ticketing.Application.Carts.UseCases.Clear;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Ticketing.Presentation.Carts
{
    internal sealed class ClearCart : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/v1/carts", async (ICustomerContext customerContext, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new ClearCartCommand(customerContext.CustomerId)).ConfigureAwait(false);

                return result.Match(Results.NoContent, ApiResults.Problem);
            }).RequireAuthorization().WithTags(Tags.Carts);
        }
    }
}