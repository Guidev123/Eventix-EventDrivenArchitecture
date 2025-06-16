using Eventix.Modules.Events.Domain.Events.Errors;
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
                .NotEmpty().WithMessage(EventErrors.TitleIsRequired.Description)
                .MinimumLength(EventSpecification.TITLE_MIN_LENGTH)
                    .WithMessage(EventErrors.TitleTooShort(EventSpecification.TITLE_MIN_LENGTH).Description)
                .MaximumLength(EventSpecification.TITLE_MAX_LENGTH)
                    .WithMessage(EventErrors.TitleTooLong(EventSpecification.TITLE_MAX_LENGTH).Description);

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage(EventErrors.DescriptionIsRequired.Description)
                .MinimumLength(EventSpecification.DESCRIPTION_MIN_LENGTH)
                    .WithMessage(EventErrors.DescriptionTooShort(EventSpecification.DESCRIPTION_MIN_LENGTH).Description)
                .MaximumLength(EventSpecification.DESCRIPTION_MAX_LENGTH)
                    .WithMessage(EventErrors.DescriptionTooLong(EventSpecification.DESCRIPTION_MAX_LENGTH).Description);

            RuleFor(c => c.StartsAtUtc)
                .NotEmpty().WithMessage(EventErrors.StartDateIsRequired.Description)
                .Must(c => c > DateTime.UtcNow.AddHours(MINIMUM_START_TIME))
                    .WithMessage(EventErrors.StartDateTooSoon(MINIMUM_START_TIME).Description);

            RuleFor(c => c.EndsAtUtc)
                .Must((cmd, endsAt) => endsAt > cmd.StartsAtUtc)
                .When(c => c.EndsAtUtc.HasValue)
                .WithMessage(EventErrors.EndDateBeforeStartDate.Description);
        }
    }
}