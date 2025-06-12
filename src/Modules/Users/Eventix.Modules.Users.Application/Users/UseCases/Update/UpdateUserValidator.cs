using FluentValidation;

namespace Eventix.Modules.Users.Application.Users.UseCases.Update
{
    public sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID cannot be empty");
            RuleFor(x => x.FirstName).MaximumLength(50).WithMessage("First name cannot exceed 50 characters");
            RuleFor(x => x.LastName).MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Invalid email format");
        }
    }
}