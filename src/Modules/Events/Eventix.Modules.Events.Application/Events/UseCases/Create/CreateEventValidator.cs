using Eventix.Modules.Events.Domain.Events.Entities;
using Eventix.Modules.Events.Domain.Events.ValueObjects;
using FluentValidation;

namespace Eventix.Modules.Events.Application.Events.UseCases.Create
{
    internal sealed class CreateEventValidator : AbstractValidator<CreateEventCommand>
    {
        private const int MINIMUM_START_TIME = 1;

        public CreateEventValidator()
        {
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(EventSpecification.TITLE_MIN_LENGTH)
                .WithMessage($"Title must be at least {EventSpecification.TITLE_MIN_LENGTH} characters long.")
                .MaximumLength(EventSpecification.TITLE_MAX_LENGTH)
                .WithMessage($"Title must not exceed {EventSpecification.DESCRIPTION_MAX_LENGTH} characters.");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(EventSpecification.DESCRIPTION_MIN_LENGTH)
                .WithMessage($"Description must be at least {EventSpecification.DESCRIPTION_MIN_LENGTH} characters long.")
                .MaximumLength(EventSpecification.DESCRIPTION_MAX_LENGTH)
                .WithMessage("Description must not exceed {EventSpecification.DESCRIPTION_MAX_LENGTH} characters.");

            RuleFor(c => c.StartsAtUtc).NotEmpty().WithMessage("Start date must be not empty.")
                .Must(c => c > DateTime.UtcNow.AddHours(MINIMUM_START_TIME)).WithMessage($"The event must start at least {MINIMUM_START_TIME} hour later.");

            RuleFor(c => c.EndsAtUtc).Must((cmd, endsAt) => endsAt > cmd.StartsAtUtc).When(c => c.EndsAtUtc.HasValue);
        }
    }
}