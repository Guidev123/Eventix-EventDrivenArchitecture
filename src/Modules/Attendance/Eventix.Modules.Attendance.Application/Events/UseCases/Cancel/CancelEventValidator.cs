using Eventix.Modules.Attendance.Domain.Events.Errors;
using FluentValidation;

namespace Eventix.Modules.Attendance.Application.Events.UseCases.Cancel
{
    internal sealed class CancelEventValidator : AbstractValidator<CancelEventCommand>
    {
        public CancelEventValidator()
        {
            RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage(EventErrors.EventIdIsRequired.Description);
        }
    }
}