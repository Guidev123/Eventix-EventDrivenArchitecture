namespace Eventix.Modules.Users.Application.Users.UseCases.GetById
{
    public record GetUserByIdResponse(
        string FirstName,
        string LastName,
        string Email
    );
}