using Eventix.Modules.Events.Domain.Events.Errors;
using FluentValidation;

namespace Eventix.Modules.Events.Application.Events.UseCases.Publish
{
    internal sealed class PublishEventValidator : AbstractValidator<PublishEventCommand>
    {
        public PublishEventValidator()
        {
            RuleFor(c => c.EventId).NotEmpty().WithMessage(EventErrors.EventIdIsRequired.Description);
        }
    }
}