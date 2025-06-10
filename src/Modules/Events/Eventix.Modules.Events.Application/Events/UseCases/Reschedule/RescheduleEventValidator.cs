using FluentValidation;

namespace Eventix.Modules.Events.Application.Events.UseCases.Reschedule
{
    public sealed class RescheduleEventValidator : AbstractValidator<RescheduleEventCommand>
    {
        private const int MINIMUM_START_TIME = 1;

        public RescheduleEventValidator()
        {
            RuleFor(c => c.EventId).NotEmpty().WithMessage("Event ID must be not empty.");

            RuleFor(c => c.StartsAtUtc).NotEmpty().WithMessage("Start date must be not empty.")
                .Must(c => c > DateTime.UtcNow.AddHours(MINIMUM_START_TIME)).WithMessage($"The event must start at least {MINIMUM_START_TIME} hour later.");
            RuleFor(c => c.EndsAtUtc).Must((cmd, endsAt) => endsAt > cmd.StartsAtUtc).When(c => c.EndsAtUtc.HasValue);
        }
    }
}