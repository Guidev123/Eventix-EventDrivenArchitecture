using Eventix.Modules.Users.Application.Users.UseCases.GetPermissions;
using Eventix.Shared.Application.Authorization;
using Eventix.Shared.Domain.Responses;
using MidR.Interfaces;

namespace Eventix.Modules.Users.Infrastructure.Authorization
{
    internal sealed class PermissionService(IMediator mediator) : IPermissionService
    {
        public async Task<Result<PermissionResponse>> GetUserPermissionsAsync(string identityId)
        {
            return await mediator.DispatchAsync(new GetUserPermissionsQuery(identityId)).ConfigureAwait(false);
        }
    }
}