using Eventix.Modules.Users.Domain.Users.Errors;
using Eventix.Shared.Domain.ValueObjects;
using FluentValidation;

namespace Eventix.Modules.Users.Application.Users.UseCases.Update
{
    internal sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(UserErrors.UserIdEmpty.Description);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(Name.NAME_MAX_LENGTH)
                .WithMessage(UserErrors.FirstNameTooLong.Description);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(Name.NAME_MAX_LENGTH)
                .WithMessage(UserErrors.LastNameTooLong.Description);
        }
    }
}