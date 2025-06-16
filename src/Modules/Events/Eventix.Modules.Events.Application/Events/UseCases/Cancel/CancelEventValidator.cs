using Eventix.Modules.Events.Domain.Events.Errors;
using FluentValidation;

namespace Eventix.Modules.Events.Application.Events.UseCases.Cancel
{
    public sealed class CancelEventValidator : AbstractValidator<CancelEventCommand>
    {
        public CancelEventValidator()
        {
            RuleFor(x => x.EventId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage(EventErrors.EventIdIsRequired.Description);
        }
    }
}