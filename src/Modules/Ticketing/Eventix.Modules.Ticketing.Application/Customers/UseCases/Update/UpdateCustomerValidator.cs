using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Shared.Domain.ValueObjects;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.Update
{
    internal sealed class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage(CustomerErrors.CustomerIdCannotBeEmpty.Description);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(Name.NAME_MAX_LENGTH)
                .WithMessage(CustomerErrors.FirstNameMaxLengthExceeded.Description);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(Name.NAME_MAX_LENGTH)
                .WithMessage(CustomerErrors.LastNameMaxLengthExceeded.Description);
        }
    }
}