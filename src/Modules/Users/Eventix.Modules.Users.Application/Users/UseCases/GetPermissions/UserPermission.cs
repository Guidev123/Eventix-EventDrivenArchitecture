namespace Eventix.Modules.Users.Application.Users.UseCases.GetPermissions
{
    internal sealed record UserPermission(Guid UserId, string Permission);
}