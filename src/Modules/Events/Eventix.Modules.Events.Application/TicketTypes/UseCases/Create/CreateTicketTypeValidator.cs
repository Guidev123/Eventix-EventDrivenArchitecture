using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.TicketTypes.Errors;
using Eventix.Shared.Domain.ValueObjects;
using Eventix.Shared.Domain.ValueObjects.Errors;
using FluentValidation;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.Create
{
    internal sealed class CreateTicketTypeValidator : AbstractValidator<CreateTicketTypeCommand>
    {
        public CreateTicketTypeValidator()
        {
            RuleFor(c => c.EventId)
                .NotEmpty()
                .WithMessage(EventErrors.EventIdIsRequired.Description);

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(TicketTypeErrors.NameIsRequired.Description)
                .MaximumLength(Name.NAME_MAX_LENGTH)
                .WithMessage(TicketTypeErrors.NameTooLong.Description);

            RuleFor(c => c.Price)
                .GreaterThan(decimal.Zero)
                .WithMessage(ValueObjectErrors.PriceMustBeGreaterThanZero.Description);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .WithMessage(ValueObjectErrors.CurrencyIsRequired.Description)
                .Length(Money.CURRENCY_CODE_LEN)
                .WithMessage(ValueObjectErrors.InvalidCurrencyLength.Description)
                .Matches(Money.CURRENCY_CODE_PATTERN)
                .WithMessage(ValueObjectErrors.InvalidCurrencyFormat.Description);

            RuleFor(c => c.Quantity)
                .GreaterThan(decimal.Zero)
                .WithMessage(TicketTypeErrors.QuantityMustBeGreaterThanZero.Description);
        }
    }
}