using Eventix.Modules.Ticketing.Domain.Events.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Events.UseCases.Reschedule
{
    internal sealed class RescheduleEventValidator : AbstractValidator<RescheduleEventCommand>
    {
        public RescheduleEventValidator()
        {
            RuleFor(x => x.EventId)
                .NotEmpty()
                .WithMessage(EventErrors.EventIdIsRequired.Description);

            RuleFor(x => x.StartsAtUtc)
                .Must(BeInValidRange)
                .WithMessage(EventErrors.StartDateMustBeInFuture.Description);

            RuleFor(x => x)
                .Must(HaveEndDateAfterStartDate)
                .When(x => x.EndsAtUtc.HasValue)
                .WithMessage(EventErrors.EndDateMustBeAfterStartDate.Description)
                .WithName(nameof(RescheduleEventCommand.EndsAtUtc));
        }

        private bool BeInValidRange(DateTime startsAtUtc)
        {
            return startsAtUtc > DateTime.UtcNow;
        }

        private bool HaveEndDateAfterStartDate(RescheduleEventCommand command)
        {
            if (!command.EndsAtUtc.HasValue)
                return true;

            return command.EndsAtUtc.Value > command.StartsAtUtc;
        }
    }
}