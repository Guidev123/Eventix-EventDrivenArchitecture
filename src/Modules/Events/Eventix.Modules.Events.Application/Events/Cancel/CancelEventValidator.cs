using FluentValidation;

namespace Eventix.Modules.Events.Application.Events.Cancel
{
    public sealed class CancelEventValidator : AbstractValidator<CancelEventCommand>
    {
        public CancelEventValidator()
        {
            RuleFor(x => x.EventId)
                .NotEmpty()
                .WithMessage("Event ID must not be empty.")
                .NotEqual(Guid.Empty)
                .WithMessage("Event ID must not be an empty GUID.");
        }
    }
}