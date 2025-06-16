using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.Update
{
    public sealed class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID cannot be empty");
            RuleFor(x => x.FirstName).MaximumLength(50).WithMessage("First name cannot exceed 50 characters");
            RuleFor(x => x.LastName).MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Invalid email format");
        }
    }
}