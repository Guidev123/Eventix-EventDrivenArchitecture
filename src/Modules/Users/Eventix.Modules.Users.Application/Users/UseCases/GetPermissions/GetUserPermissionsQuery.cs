using Eventix.Shared.Application.Authorization;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Users.Application.Users.UseCases.GetPermissions
{
    public sealed record GetUserPermissionsQuery(string IdentiyProviderId) : IQuery<PermissionResponse>;
}