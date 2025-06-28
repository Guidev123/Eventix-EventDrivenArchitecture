using Eventix.Shared.Application.Exceptions;
using System.Security.Claims;

namespace Eventix.Shared.Infrastructure.Authentication
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal?.FindFirst(CustomClaims.SUB)?.Value;
            return Guid.TryParse(userId, out var parsedUserId)
                ? parsedUserId
                : throw new EventixException("User identifier is unavaible");
        }

        public static string GetIdentityId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new EventixException("User identity is unavaible");
        }

        public static HashSet<string> GetPermissions(this ClaimsPrincipal claimsPrincipal)
        {
            var permissionClaims = claimsPrincipal?.FindAll(CustomClaims.PERMISSION)
                ?? throw new EventixException("Permissions are unavaible");

            return permissionClaims.Select(c => c.Value).ToHashSet();
        }
    }
}