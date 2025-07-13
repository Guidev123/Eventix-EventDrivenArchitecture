using Eventix.Modules.Users.Application.Users.UseCases.GetById;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Infrastructure.Authentication;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace Eventix.Modules.Users.Presentation.Users
{
    internal sealed class GetUserByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/users/profile", async (ClaimsPrincipal claims, IMediatorHandler mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetUserByIdQuery(claims.GetUserId())).ConfigureAwait(false);

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetUser).WithTags(Tags.Users);
        }
    }
}