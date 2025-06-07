using FluentValidation;

namespace Eventix.Modules.Events.Application.TicketTypes.UpdatePrice
{
    public sealed class UpdateTicketTypePriceValidator : AbstractValidator<UpdateTicketTypePriceCommand>
    {
        public UpdateTicketTypePriceValidator()
        {
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}