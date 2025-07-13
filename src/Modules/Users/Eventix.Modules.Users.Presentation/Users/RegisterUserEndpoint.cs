using Eventix.Modules.Users.Application.Users.UseCases.Register;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Users.Presentation.Users
{
    internal sealed class RegisterUserEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/users", async (RegisterUserCommand command, IMediatorHandler mediator) =>
            {
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return result.Match(
                    success => Results.Created($"/users/{result.Value.UserId}", result.Value),
                    failure => ApiResults.Problem(failure)
                );
            }).WithTags(Tags.Users);
        }
    }
}