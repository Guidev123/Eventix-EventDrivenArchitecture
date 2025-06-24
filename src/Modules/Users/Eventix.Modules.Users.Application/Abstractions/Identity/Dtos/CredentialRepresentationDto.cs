namespace Eventix.Modules.Users.Application.Abstractions.Identity.Dtos
{
    public sealed record CredentialRepresentationDto(
        string Type,
        string Value,
        bool Temporary
        );
}