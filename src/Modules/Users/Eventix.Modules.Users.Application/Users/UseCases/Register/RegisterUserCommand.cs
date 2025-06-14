using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Users.Application.Users.UseCases.Register
{
    public record RegisterUserCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string ConfirmPassword
        ) : ICommand<RegisterUserResponse>;
}