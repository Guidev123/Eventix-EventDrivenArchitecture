using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Shared.Domain.ValueObjects;
using Eventix.Shared.Domain.ValueObjects.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.TicketTypes.UseCases.UpdatePrice
{
    internal sealed class UpdateTicketTypePriceValidator : AbstractValidator<UpdateTicketTypePriceCommand>
    {
        public UpdateTicketTypePriceValidator()
        {
            RuleFor(p => p.TicketTypeId).NotEmpty().WithMessage(TicketTypeErrors.InvalidTicketTypeId.Description);
            RuleFor(p => p.Price).NotEmpty().LessThan(Money.MINIMUM_AMOUNT).WithMessage(ValueObjectErrors.PriceMustBeGreaterThanZero.Description);
        }
    }
}