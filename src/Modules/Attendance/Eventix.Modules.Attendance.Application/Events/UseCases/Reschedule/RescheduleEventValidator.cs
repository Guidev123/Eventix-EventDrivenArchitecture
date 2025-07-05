using Eventix.Modules.Attendance.Domain.Events.Errors;
using FluentValidation;

namespace Eventix.Modules.Attendance.Application.Events.UseCases.Reschedule
{
    internal sealed class RescheduleEventValidator : AbstractValidator<RescheduleEventCommand>
    {
        public RescheduleEventValidator()
        {
            RuleFor(x => x.EventId)
                .NotEmpty()
                .WithMessage(EventErrors.EventIdIsRequired.Description);

            RuleFor(x => x.StartsAtUtc)
                .NotEmpty()
                .WithMessage(EventErrors.InvalidStartDate.Description);

            RuleFor(c => c.EndsAtUtc).Must((cmd, endsAt) => endsAt > cmd.StartsAtUtc).When(c => c.EndsAtUtc.HasValue);
        }
    }
}