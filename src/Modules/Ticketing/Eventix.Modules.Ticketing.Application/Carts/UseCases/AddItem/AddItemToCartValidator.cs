using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.AddItem
{
    public sealed class AddItemToCartValidator : AbstractValidator<AddItemToCartCommand>
    {
        private const int MINIMUM_QUANTITY = 1;

        public AddItemToCartValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID must not be empty");
            RuleFor(x => x.TicketTypeId).NotEmpty().WithMessage("Ticket Type ID must not be empty");
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(MINIMUM_QUANTITY).WithMessage($"Quantity must be greater than or equal to {MINIMUM_QUANTITY}");
        }
    }
}