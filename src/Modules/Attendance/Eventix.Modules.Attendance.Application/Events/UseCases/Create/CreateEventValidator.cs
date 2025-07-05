using Eventix.Modules.Attendance.Domain.Events.Errors;
using FluentValidation;

namespace Eventix.Modules.Attendance.Application.Events.UseCases.Create
{
    internal sealed class CreateEventValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventValidator()
        {
            RuleFor(x => x.EventId)
                .NotEmpty()
                .WithMessage(EventErrors.InvalidEventId.Description);

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(EventErrors.TitleIsRequired.Description);

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(EventErrors.DescriptionIsRequired.Description);

            RuleFor(c => c.StartsAtUtc)
                .NotEmpty().WithMessage(EventErrors.InvalidStartDate.Description);

            When(x => x.EndsAtUtc.HasValue, () =>
            {
                RuleFor(x => x.EndsAtUtc!.Value)
                    .GreaterThanOrEqualTo(x => x.StartsAtUtc)
                    .WithMessage(EventErrors.EndDateMustBeAfterStartDate.Description);
            });
        }
    }
}