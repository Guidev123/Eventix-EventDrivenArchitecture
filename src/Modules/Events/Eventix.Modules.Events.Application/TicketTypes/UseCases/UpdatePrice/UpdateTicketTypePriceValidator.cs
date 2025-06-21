using Eventix.Modules.Events.Domain.TicketTypes.Errors;
using Eventix.Shared.Domain.ValueObjects.Errors;
using FluentValidation;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.UpdatePrice
{
    public sealed class UpdateTicketTypePriceValidator : AbstractValidator<UpdateTicketTypePriceCommand>
    {
        public UpdateTicketTypePriceValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage(ValueObjectErrors.PriceMustBeGreaterThanZero.Description);
        }
    }
}