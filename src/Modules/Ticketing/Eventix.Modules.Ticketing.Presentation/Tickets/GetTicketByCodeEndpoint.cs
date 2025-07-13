using Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetByCode;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Ticketing.Presentation.Tickets
{
    internal sealed class GetTicketByCodeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/tickets/code/{code}", async (string code, IMediatorHandler mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetTicketByCodeQuery(code));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetTickets)
            .WithTags(Tags.Tickets);
        }
    }
}