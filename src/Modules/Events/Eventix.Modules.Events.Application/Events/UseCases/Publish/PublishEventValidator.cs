using FluentValidation;

namespace Eventix.Modules.Events.Application.Events.UseCases.Publish
{
    public sealed class PublishEventValidator : AbstractValidator<PublishEventCommand>
    {
        public PublishEventValidator()
        {
            RuleFor(c => c.EventId).NotEmpty().WithMessage("Event ID must be not empty.");
        }
    }
}