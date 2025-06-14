using Eventix.Modules.Users.Application.Users.UseCases.GetById;
using Eventix.Modules.Users.PublicApi;
using Eventix.Modules.Users.PublicApi.Users.Responses;
using MidR.Interfaces;

namespace Eventix.Modules.Users.Infrastructure.PublicApi
{
    internal sealed class UsersApi(IMediator mediator) : IUsersApi
    {
        public async Task<UserResponse?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var result = await mediator.DispatchAsync(new GetUserByIdQuery(userId), cancellationToken).ConfigureAwait(false);
            if (result.IsFailure) return null;

            return new UserResponse(
                result.Value.Id,
                result.Value.FirstName,
                result.Value.LastName,
                result.Value.Email
            );
        }
    }
}