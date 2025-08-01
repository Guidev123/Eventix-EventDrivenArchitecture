﻿using Eventix.Modules.Events.Application.TicketTypes.UseCases.Create;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Presentation.TicketTypes
{
    internal sealed class CreateTicketTypeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/events/ticket-types", async (CreateTicketTypeCommand command, IMediatorHandler mediator) =>
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