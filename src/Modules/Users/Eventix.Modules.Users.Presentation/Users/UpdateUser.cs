using Eventix.Modules.Users.Application.Users.UseCases.Update;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Users.Presentation.Users
{
    internal sealed class UpdateUser : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1/users/{id:guid}", async (Guid id, UpdateUserCommand command, IMediator mediator) =>
            {
                command.SetUserId(id);
                return (await mediator
                .DispatchAsync(command)
                .ConfigureAwait(false))
                .Match(Results.NoContent, ApiResults.Problem);
            }).RequireAuthorization().WithTags(Tags.Users);
        }
    }
}