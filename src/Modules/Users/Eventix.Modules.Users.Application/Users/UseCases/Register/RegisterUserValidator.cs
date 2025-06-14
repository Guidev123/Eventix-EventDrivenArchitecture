using FluentValidation;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

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

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The password field cannot be empty.")
                .MinimumLength(8).WithMessage("The password must be at least 8 characters long.")
                .Must(HasUpperCase).WithMessage("The password must contain at least one uppercase letter.")
                .Must(HasLowerCase).WithMessage("The password must contain at least one lowercase letter.")
                .Must(HasDigit).WithMessage("The password must contain at least one digit.")
                .Must(HasSpecialCharacter).WithMessage("The password must contain at least one special character (!@#$%^&* etc.).");

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("The passwords do not match")
                    .When(x => !string.IsNullOrEmpty(x.Password))
                    .NotEmpty().WithMessage("The password field cannot be empty.")
                    .MinimumLength(8).WithMessage("The password must be at least 8 characters long.");
        }

        private static bool HasUpperCase(string password) => password.Any(char.IsUpper);

        private static bool HasLowerCase(string password) => password.Any(char.IsLower);

        private static bool HasDigit(string password) => password.Any(char.IsDigit);

        private static bool HasSpecialCharacter(string password)
        {
            var specialCharRegex = new Regex(@"[!@#$%^&*(),.?""{}|<>]");
            return specialCharRegex.IsMatch(password);
        }
    }
}