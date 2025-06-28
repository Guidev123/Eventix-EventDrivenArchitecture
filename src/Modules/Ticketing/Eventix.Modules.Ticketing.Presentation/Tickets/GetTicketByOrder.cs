using Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetByOrder;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Ticketing.Presentation.Tickets
{
    internal sealed class GetTicketByOrder : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/tickets/order/{orderId:guid}", async (Guid orderId, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetTicketsByOrderQuery(orderId));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetTickets)
            .WithTags(Tags.Tickets);
        }
    }
}