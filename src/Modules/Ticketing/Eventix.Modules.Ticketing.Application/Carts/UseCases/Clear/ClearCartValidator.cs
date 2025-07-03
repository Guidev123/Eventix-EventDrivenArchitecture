using Eventix.Modules.Ticketing.Application.Carts.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.Clear
{
    internal sealed class ClearCartValidator : AbstractValidator<ClearCartCommand>
    {
        public ClearCartValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage(CartErrors.CustomerIdIsRequired.Description);
        }
    }
}