using Eventix.Modules.Users.Application.Users.UseCases.GetById;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Users.Presentation.Users
{
    internal sealed class GetUserById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/users/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                return (await mediator
                .DispatchAsync(new GetUserByIdQuery(id))
                .ConfigureAwait(false))
                .Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization().WithTags(Tags.Users);
        }
    }
}