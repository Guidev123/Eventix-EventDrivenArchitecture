namespace Eventix.Modules.Users.Application.Abstractions.Identity.Dtos
{
    public sealed record UserDto(
        string Email,
        string Password,
        string FirstName,
        string LastName
        );
}