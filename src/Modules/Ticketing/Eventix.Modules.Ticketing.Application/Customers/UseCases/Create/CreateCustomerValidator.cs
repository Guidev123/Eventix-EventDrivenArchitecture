using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.Create
{
    public sealed class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage(CustomerErrors.CustomerIdIsRequired.Description);

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage(CustomerErrors.InvalidEmailFormat.Description);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(2, 50)
                .WithMessage(CustomerErrors.FirstNameLengthInvalid.Description);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(2, 50)
                .WithMessage(CustomerErrors.LastNameLengthInvalid.Description);
        }
    }
}