using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.Create
{
    public sealed class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID must not be empty");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid e-mail format");
            RuleFor(x => x.FirstName).NotEmpty().Length(2, 50).WithMessage("First Name must be betwen 2 and 50 caracters");
            RuleFor(x => x.LastName).NotEmpty().Length(2, 50).WithMessage("Last Name must be betwen 2 and 50 caracters");
        }
    }
}