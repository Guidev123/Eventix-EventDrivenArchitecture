using Dapper;
using Eventix.Modules.Users.Domain.Users.Errors;
using Eventix.Shared.Application.Authorization;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Users.Application.Users.UseCases.GetPermissions
{
    internal sealed class GetUserPermissionsHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetUserPermissionsQuery, PermissionResponse>
    {
        public async Task<Result<PermissionResponse>> ExecuteAsync(GetUserPermissionsQuery request, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();

            const string sql = $@"
                SELECT DISTINCT
                    u.Id,
                    rp.PermissionCode
                FROM users.Users u
                JOIN users.UserRoles ur ON ur.UserId = u.Id
                JOIN users.RolePermissions rp ON rp.RoleName = ur.RoleName
                WHERE u.IdentiyProviderId = @IdentiyProviderId
                ";

            var permissions = (await connection.QueryAsync<UserPermission>(sql, request)).AsList();

            if (permissions.Count == 0)
                return Result.Failure<PermissionResponse>(UserErrors.NotFound(request.IdentiyProviderId));

            PermissionResponse permissionResponse = new(permissions[0].Id, permissions.Select(c => c.PermissionCode).ToHashSet());

            return Result.Success(permissionResponse);
        }
    }
}