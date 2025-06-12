using FluentValidation;

namespace Eventix.Modules.Users.Application.Users.UseCases.Register
{
    public sealed class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(command => command.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters");

            RuleFor(command => command.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters");

            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(160).WithMessage("Email must not exceed 160 characters");

            RuleFor(command => command.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long");
        }
    }
}