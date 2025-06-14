using Eventix.Modules.Ticketing.Application.Carts.UseCases.AddItemToCart;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Ticketing.Presentation.Carts
{
    internal sealed class AddToCart : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/carts/item", async (AddItemToCartCommand command, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Carts);
        }
    }
}