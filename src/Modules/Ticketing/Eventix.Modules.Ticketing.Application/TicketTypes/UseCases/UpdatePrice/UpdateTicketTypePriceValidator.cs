using Eventix.Modules.Ticketing.Domain.Events.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.TicketTypes.UseCases.UpdatePrice
{
    internal sealed class UpdateTicketTypePriceValidator : AbstractValidator<UpdateTicketTypePriceCommand>
    {
        public UpdateTicketTypePriceValidator()
        {
            RuleFor(p => p.TicketTypeId)
                .NotEmpty()
                .WithMessage(TicketTypeErrors.InvalidTicketTypeId.Description);

            RuleFor(p => p.Price)
                .NotEmpty()
                .WithMessage(TicketTypeErrors.PriceIsRequired.Description);
        }
    }
}