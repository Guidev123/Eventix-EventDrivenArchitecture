using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.Update
{
    public sealed class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage(CustomerErrors.CustomerIdCannotBeEmpty.Description);

            RuleFor(x => x.FirstName)
                .MaximumLength(50)
                .WithMessage(CustomerErrors.FirstNameMaxLengthExceeded.Description);

            RuleFor(x => x.LastName)
                .MaximumLength(50)
                .WithMessage(CustomerErrors.LastNameMaxLengthExceeded.Description);

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage(CustomerErrors.InvalidEmailFormat.Description);
        }
    }
}