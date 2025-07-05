using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.Events.ValueObjects;
using Eventix.Shared.Domain.ValueObjects;
using FluentValidation;

namespace Eventix.Modules.Events.Application.Events.UseCases.Create
{
    internal sealed class CreateEventValidator : AbstractValidator<CreateEventCommand>
    {
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
                .Must(c => c > DateTime.UtcNow.AddHours(DateRange.MINIMUM_START_TIME_IN_HOURS))
                    .WithMessage(EventErrors.StartDateTooSoon(DateRange.MINIMUM_START_TIME_IN_HOURS).Description);

            When(x => x.EndsAtUtc.HasValue, () =>
            {
                RuleFor(x => x.EndsAtUtc!.Value)
                    .NotEqual(default(DateTime))
                    .WithMessage(EventErrors.InvalidEndDate.Description)
                    .GreaterThanOrEqualTo(x => x.StartsAtUtc)
                    .WithMessage(EventErrors.EndDateMustBeAfterStartDate.Description);
            });
        }
    }
}