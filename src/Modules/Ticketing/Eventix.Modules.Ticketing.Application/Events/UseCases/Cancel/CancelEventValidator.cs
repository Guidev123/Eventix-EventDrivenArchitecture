using Eventix.Modules.Ticketing.Domain.Events.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Events.UseCases.Cancel
{
    public sealed class CancelEventValidator : AbstractValidator<CancelEventCommand>
    {
        public CancelEventValidator()
        {
            RuleFor(e => e.EventId).NotEmpty().WithMessage(EventErrors.EventIdIsRequired.Description);
        }
    }
}