using Eventix.Modules.Ticketing.Application.Carts.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.AddItem
{
    public sealed class AddItemToCartValidator : AbstractValidator<AddItemToCartCommand>
    {
        private const int MINIMUM_QUANTITY = 1;

        public AddItemToCartValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage(CartErrors.CustomerIdIsRequired.Description);

            RuleFor(x => x.TicketTypeId)
                .NotEmpty()
                .WithMessage(CartErrors.TicketTypeIdIsRequired.Description);

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(MINIMUM_QUANTITY)
                .WithMessage(CartErrors.QuantityMustBeAtLeast(MINIMUM_QUANTITY).Description);
        }
    }
}