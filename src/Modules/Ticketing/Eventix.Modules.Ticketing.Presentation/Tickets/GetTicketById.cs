using Eventix.Modules.Ticketing.Application.Tickets.UseCases.GetById;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Ticketing.Presentation.Tickets
{
    internal sealed class GetTicketById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/tickets/{id}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetTicketByIdQuery(id));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization()
            .WithTags(Tags.Tickets);
        }
    }
}