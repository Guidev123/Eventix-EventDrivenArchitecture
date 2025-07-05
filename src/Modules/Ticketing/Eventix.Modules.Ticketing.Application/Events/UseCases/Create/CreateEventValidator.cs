using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Shared.Domain.ValueObjects;
using Eventix.Shared.Domain.ValueObjects.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Events.UseCases.Create
{
    internal sealed class CreateEventValidator : AbstractValidator<CreateEventCommand>
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

            RuleFor(c => c.StartsAtUtc)
                .NotEmpty().WithMessage(EventErrors.InvalidStartDate.Description);

            When(x => x.EndsAtUtc.HasValue, () =>
            {
                RuleFor(x => x.EndsAtUtc!.Value)
                    .GreaterThanOrEqualTo(x => x.StartsAtUtc)
                    .WithMessage(EventErrors.EndDateMustBeAfterStartDate.Description);
            });

            When(x => x.TicketTypes.Count > 0, () =>
            {
                RuleForEach(x => x.TicketTypes)
                    .SetValidator(new TicketTypeRequestValidator());
            });
        }

        internal sealed class TicketTypeRequestValidator : AbstractValidator<CreateEventCommand.TicketTypeRequest>
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
                    .GreaterThan(decimal.Zero)
                    .WithMessage(TicketTypeErrors.PriceMustBeGreaterThanZero.Description);

                RuleFor(x => x.Currency)
                    .NotEmpty()
                    .WithMessage(TicketTypeErrors.CurrencyIsRequired.Description);

                RuleFor(x => x.Quantity)
                    .GreaterThan(decimal.Zero)
                    .WithMessage(TicketTypeErrors.QuantityMustBeGreaterThanZero.Description);
            }
        }
    }
}