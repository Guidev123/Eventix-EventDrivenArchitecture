using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Shared.Domain.ValueObjects;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.Create
{
    internal sealed class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
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
                .Length(Name.NAME_MIN_LENGTH, Name.NAME_MAX_LENGTH)
                .WithMessage(CustomerErrors.FirstNameLengthInvalid.Description);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(Name.NAME_MIN_LENGTH, Name.NAME_MAX_LENGTH)
                .WithMessage(CustomerErrors.LastNameLengthInvalid.Description);
        }
    }
}