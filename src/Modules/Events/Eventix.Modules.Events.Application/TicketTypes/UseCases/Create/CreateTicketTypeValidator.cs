using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.TicketTypes.Errors;
using Eventix.Modules.Events.Domain.TicketTypes.ValueObjects;
using Eventix.Shared.Domain.ValueObjects.Errors;
using FluentValidation;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.Create
{
    public sealed class CreateTicketTypeValidator : AbstractValidator<CreateTicketTypeCommand>
    {
        public CreateTicketTypeValidator()
        {
            RuleFor(c => c.EventId)
                .NotEmpty()
                .WithMessage(EventErrors.EventIdIsRequired.Description);

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage(TicketTypeErrors.NameIsRequired.Description)
                .MaximumLength(100).WithMessage(TicketTypeErrors.NameTooLong.Description);

            RuleFor(c => c.Price)
                .GreaterThan(0).WithMessage(TicketTypeErrors.PriceMustBeGreaterThanZero.Description);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .WithMessage(ValueObjectErrors.CurrencyIsRequired.Description)
                .Length(Money.MIN_CURRENCY_LENGTH)
                .WithMessage(ValueObjectErrors.InvalidCurrencyLength.Description)
                .Matches(Money.CURRENCY_CODE_PATTERN)
                .WithMessage(ValueObjectErrors.InvalidCurrencyFormat.Description);

            RuleFor(c => c.Quantity)
                .GreaterThan(0).WithMessage(TicketTypeErrors.QuantityMustBeGreaterThanZero.Description);
        }
    }
}