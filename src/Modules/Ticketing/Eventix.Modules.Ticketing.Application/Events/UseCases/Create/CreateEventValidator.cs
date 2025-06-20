using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Shared.Domain.ValueObjects;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Events.UseCases.Create
{
    public sealed class CreateEventValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventValidator()
        {
            RuleFor(x => x.EventId)
                .NotEmpty()
                .WithMessage(EventErrors.InvalidEventId.Description);

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(EventErrors.TitleIsRequired.Description);

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(EventErrors.DescriptionIsRequired.Description);

            RuleFor(x => x.Location)
                .NotNull()
                .WithMessage(EventErrors.LocationIsRequired.Description);

            RuleFor(x => x.StartsAtUtc)
                .NotEqual(default(DateTime))
                .WithMessage(EventErrors.InvalidStartDate.Description);

            When(x => x.EndsAtUtc.HasValue, () =>
            {
                RuleFor(x => x.EndsAtUtc!.Value)
                    .NotEqual(default(DateTime))
                    .WithMessage(EventErrors.InvalidEndDate.Description)
                    .GreaterThanOrEqualTo(x => x.StartsAtUtc)
                    .WithMessage(EventErrors.EndDateMustBeAfterStartDate.Description);
            });

            RuleFor(x => x.TicketTypes)
                .NotNull()
                .WithMessage(EventErrors.TicketTypesIsRequired.Description)
                .NotEmpty()
                .WithMessage(EventErrors.TicketTypesCannotBeEmpty.Description);

            RuleForEach(x => x.TicketTypes)
                .SetValidator(new TicketTypeRequestValidator());
        }

        private class TicketTypeRequestValidator : AbstractValidator<CreateEventCommand.TicketTypeRequest>
        {
            public TicketTypeRequestValidator()
            {
                RuleFor(x => x.TicketTypeId)
                    .NotEmpty()
                    .WithMessage(TicketTypeErrors.InvalidTicketTypeId.Description);

                RuleFor(x => x.EventId)
                    .NotEmpty()
                    .WithMessage(TicketTypeErrors.InvalidEventId.Description);

                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage(TicketTypeErrors.NameIsRequired.Description);

                RuleFor(x => x.Price)
                    .GreaterThan(0)
                    .WithMessage(TicketTypeErrors.PriceMustBeGreaterThanZero.Description);

                RuleFor(x => x.Currency)
                    .NotEmpty()
                    .WithMessage(TicketTypeErrors.CurrencyIsRequired.Description)
                    .Length(Money.MIN_CURRENCY_LENGTH)
                    .WithMessage(TicketTypeErrors.InvalidCurrencyLength.Description)
                    .Matches(Money.CURRENCY_CODE_PATTERN)
                    .WithMessage(TicketTypeErrors.InvalidCurrencyFormat.Description);

                RuleFor(x => x.Quantity)
                    .GreaterThan(0)
                    .WithMessage(TicketTypeErrors.QuantityMustBeGreaterThanZero.Description);
            }
        }
    }
}