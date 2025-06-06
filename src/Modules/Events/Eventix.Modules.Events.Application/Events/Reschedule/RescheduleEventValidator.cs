using FluentValidation;

namespace Eventix.Modules.Events.Application.Events.Reschedule
{
    public sealed class RescheduleEventValidator : AbstractValidator<RescheduleEventCommand>
    {
        public RescheduleEventValidator()
        {
            RuleFor(c => c.EventId).NotEmpty();
            RuleFor(c => c.StartsAtUtc).NotEmpty();
            RuleFor(c => c.EndsAtUtc).Must((cmd, endsAt) => endsAt > cmd.StartsAtUtc).When(c => c.EndsAtUtc.HasValue);
        }
    }
}