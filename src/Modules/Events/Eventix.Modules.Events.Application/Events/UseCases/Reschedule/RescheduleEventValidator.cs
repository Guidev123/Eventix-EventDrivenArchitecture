using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Shared.Domain.ValueObjects;
using FluentValidation;

namespace Eventix.Modules.Events.Application.Events.UseCases.Reschedule
{
    internal sealed class RescheduleEventValidator : AbstractValidator<RescheduleEventCommand>
    {
        public RescheduleEventValidator()
        {
            RuleFor(c => c.EventId)
                .NotEmpty()
                .WithMessage(EventErrors.EventIdIsRequired.Description);

            RuleFor(c => c.StartsAtUtc)
                .NotEmpty()
                .WithMessage(EventErrors.StartDateIsRequired.Description)
                .Must(c => c > DateTime.UtcNow.AddHours(DateRange.MINIMUM_START_TIME_IN_HOURS))
                .WithMessage(EventErrors.StartDateTooSoon(DateRange.MINIMUM_START_TIME_IN_HOURS).Description);

            RuleFor(c => c.EndsAtUtc)
                .Must((cmd, endsAt) => endsAt > cmd.StartsAtUtc)
                .When(c => c.EndsAtUtc.HasValue)
                .WithMessage(EventErrors.EndDateMustBeAfterStartDate.Description);
        }
    }
}