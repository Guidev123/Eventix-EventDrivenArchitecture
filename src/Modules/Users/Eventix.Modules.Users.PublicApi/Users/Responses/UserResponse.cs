namespace Eventix.Modules.Users.PublicApi.Users.Responses
{
    public record UserResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email
    );
}