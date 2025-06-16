using Eventix.Modules.Users.Domain.Users.Errors;
using FluentValidation;

namespace Eventix.Modules.Users.Application.Users.UseCases.Update
{
    public sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(UserErrors.UserIdEmpty.Description);

            RuleFor(x => x.FirstName)
                .MaximumLength(50)
                .WithMessage(UserErrors.FirstNameTooLong.Description);

            RuleFor(x => x.LastName)
                .MaximumLength(50)
                .WithMessage(UserErrors.LastNameTooLong.Description);

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage(UserErrors.InvalidEmailFormat.Description);
        }
    }
}