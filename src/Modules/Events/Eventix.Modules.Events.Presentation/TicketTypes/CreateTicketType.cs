using Eventix.Modules.Events.Application.TicketTypes.UseCases.Create;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Presentation.TicketTypes
{
    internal class CreateTicketType : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/events/ticket-types", async (CreateTicketTypeCommand command, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return result.Match(
                    success => Results.Created($"/events/ticket-types/{success.Id}", success),
                    failure => ApiResults.Problem(failure)
                );
            }).RequireAuthorization(PolicyExtensions.ModifyTicketTypes)
            .WithTags(Tags.TicketTypes);
        }
    }
}