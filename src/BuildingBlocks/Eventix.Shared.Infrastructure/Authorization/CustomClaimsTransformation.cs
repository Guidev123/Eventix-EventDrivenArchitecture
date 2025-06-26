using Eventix.Shared.Application.Authorization;
using Eventix.Shared.Application.Exceptions;
using Eventix.Shared.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Eventix.Shared.Infrastructure.Authorization
{
    internal sealed class CustomClaimsTransformation(IServiceScopeFactory serviceScopeFactory) : IClaimsTransformation
    {
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.HasClaim(c => c.Type == CustomClaims.SUB))
                return principal;

            using var scope = serviceScopeFactory.CreateScope();

            var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

            var identityId = principal.GetIdentityId();

            var result = await permissionService.GetUserPermissionsAsync(identityId);

            if (result.IsFailure)
                throw new EventixException(nameof(IPermissionService.GetUserPermissionsAsync), result.Error);

            var claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new(CustomClaims.SUB, result.Value.UserId.ToString()));

            foreach (var permission in result.Value.Permissions)
            {
                claimsIdentity.AddClaim(new(CustomClaims.PERMISSION, permission));
            }

            principal.AddIdentity(claimsIdentity);

            return principal;
        }
    }
}