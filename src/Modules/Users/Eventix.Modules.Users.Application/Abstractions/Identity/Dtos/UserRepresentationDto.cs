namespace Eventix.Modules.Users.Application.Abstractions.Identity.Dtos
{
    public sealed record UserRepresentationDto(
        string Username,
        string Email,
        string FirstName,
        string LastName,
        bool EmailVerified,
        bool Enabled,
        CredentialRepresentationDto[] Credentials
        );
}