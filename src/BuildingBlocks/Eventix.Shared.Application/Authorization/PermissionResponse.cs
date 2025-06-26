namespace Eventix.Shared.Application.Authorization
{
    public sealed record PermissionResponse(
        Guid UserId,
        HashSet<string> Permissions
        );
}