using Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetByOrder;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Ticketing.Presentation.Tickets
{
    internal sealed class GetTicketByOrderEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/tickets/orders/{id:guid}", async (Guid id, IMediatorHandler mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetTicketsByOrderQuery(id));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetTickets)
            .WithTags(Tags.Tickets);
        }
    }
}