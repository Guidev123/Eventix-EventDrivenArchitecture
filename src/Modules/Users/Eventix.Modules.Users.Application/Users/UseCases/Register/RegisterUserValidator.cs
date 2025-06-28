using Eventix.Modules.Users.Domain.Users.Errors;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Eventix.Modules.Users.Application.Users.UseCases.Register
{
    internal sealed class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(command => command.FirstName)
                .NotEmpty().WithMessage(UserErrors.FirstNameIsRequired.Description)
                .MaximumLength(50).WithMessage(UserErrors.FirstNameTooLong.Description);

            RuleFor(command => command.LastName)
                .NotEmpty().WithMessage(UserErrors.LastNameIsRequired.Description)
                .MaximumLength(50).WithMessage(UserErrors.LastNameTooLong.Description);

            RuleFor(command => command.Email)
                .NotEmpty().WithMessage(UserErrors.EmailIsRequired.Description)
                .EmailAddress().WithMessage(UserErrors.EmailInvalidFormat.Description)
                .MaximumLength(160).WithMessage(UserErrors.EmailTooLong.Description);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(UserErrors.PasswordIsRequired.Description)
                .MinimumLength(8).WithMessage(UserErrors.PasswordTooShort.Description)
                .Must(HasUpperCase).WithMessage(UserErrors.PasswordMissingUpperCase.Description)
                .Must(HasLowerCase).WithMessage(UserErrors.PasswordMissingLowerCase.Description)
                .Must(HasDigit).WithMessage(UserErrors.PasswordMissingDigit.Description)
                .Must(HasSpecialCharacter).WithMessage(UserErrors.PasswordMissingSpecialChar.Description);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage(UserErrors.PasswordsDoNotMatch.Description)
                .When(x => !string.IsNullOrEmpty(x.Password))
                .NotEmpty().WithMessage(UserErrors.PasswordIsRequired.Description)
                .MinimumLength(8).WithMessage(UserErrors.PasswordTooShort.Description);
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