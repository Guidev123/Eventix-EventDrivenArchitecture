using Eventix.Modules.Ticketing.Application.Customers.UseCases.GetById;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Ticketing.Presentation.Customers
{
    internal sealed class GetCustomerById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/customers/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetCustomerByIdQuery(id)).ConfigureAwait(false);

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.Customers);
        }
    }
}