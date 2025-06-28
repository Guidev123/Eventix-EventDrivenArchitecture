using Eventix.Shared.Domain.Responses;

namespace Eventix.Shared.Application.Authorization
{
    public interface IPermissionService
    {
        Task<Result<PermissionResponse>> GetUserPermissionsAsync(string identityId, CancellationToken cancellationToken = default);
    }
}