using Eventix.Modules.Users.Application.Users.UseCases.GetPermissions;
using Eventix.Shared.Application.Authorization;
using Eventix.Shared.Application.Cache;
using Eventix.Shared.Domain.Responses;
using MidR.Interfaces;

namespace Eventix.Modules.Users.Infrastructure.Authorization
{
    internal sealed class PermissionService(IMediator mediator, ICacheService cacheService) : IPermissionService
    {
        private const int CAHCE_EXPIRATION_TIME_IN_MINUTES = 30;

        public async Task<Result<PermissionResponse>> GetUserPermissionsAsync(string identityId, CancellationToken cancellationToken = default)
        {
            var cacheResult = await cacheService.GetAsync<PermissionResponse>(identityId, cancellationToken);

            if (cacheResult is not null)
                return Result.Success(cacheResult);

            var result = await mediator.DispatchAsync(new GetUserPermissionsQuery(identityId), cancellationToken).ConfigureAwait(false);

            if (result.IsSuccess)
                await cacheService.SetAsync(identityId, result.Value, TimeSpan.FromMinutes(CAHCE_EXPIRATION_TIME_IN_MINUTES), cancellationToken);

            return result;
        }
    }
}