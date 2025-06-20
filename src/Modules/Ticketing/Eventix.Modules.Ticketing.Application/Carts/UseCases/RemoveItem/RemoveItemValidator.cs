using Eventix.Modules.Ticketing.Application.Carts.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.RemoveItem
{
    public sealed class RemoveItemValidator : AbstractValidator<RemoveItemCommand>
    {
        public RemoveItemValidator()
        {
            RuleFor(i => i.CustomerId).NotEmpty().WithMessage(CartErrors.CustomerIdIsRequired.Description);
            RuleFor(i => i.TicketTypeId).NotEmpty().WithMessage(CartErrors.TicketTypeIdIsRequired.Description);
        }
    }
}