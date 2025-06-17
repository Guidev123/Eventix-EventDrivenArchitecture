using Eventix.Modules.Ticketing.Application.Abstractions.Authentication;
using Eventix.Modules.Ticketing.Application.Carts.UseCases.AddItem;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Ticketing.Presentation.Carts
{
    internal sealed class AddItem : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/carts/item", async (ICustomerContext customerContext, AddItemToCartCommand command, IMediator mediator) =>
            {
                command.SetCustomerId(customerContext.CustomerId);
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Carts);
        }
    }
}