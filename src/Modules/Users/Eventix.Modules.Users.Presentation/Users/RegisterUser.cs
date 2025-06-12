using Eventix.Modules.Users.Application.Users.UseCases.Register;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Users.Presentation.Users
{
    internal sealed class RegisterUser : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/users", async (RegisterUserCommand command, IMediator mediator) =>
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