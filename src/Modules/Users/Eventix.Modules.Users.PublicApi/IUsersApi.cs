using Eventix.Modules.Users.PublicApi.Users.Responses;

namespace Eventix.Modules.Users.PublicApi
{
    public interface IUsersApi
    {
        Task<UserResponse?> GetAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}