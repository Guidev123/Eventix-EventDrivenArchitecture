using Eventix.Modules.Events.Domain.TicketTypes.Errors;
using FluentValidation;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.UpdatePrice
{
    public sealed class UpdateTicketTypePriceValidator : AbstractValidator<UpdateTicketTypePriceCommand>
    {
        public UpdateTicketTypePriceValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage(TicketTypeErrors.PriceMustBeGreaterThanZero.Description);
        }
    }
}