using FluentValidation;

namespace Eventix.Modules.Events.Application.Events.Create
{
    internal sealed class CreateEventValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventValidator()
        {
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(80).WithMessage("Title must not exceed 80 characters.");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(256).WithMessage("Description must not exceed 256 characters.");

            RuleFor(c => c.StartsAtUtc)
                .NotEmpty().WithMessage("Start date and time is required.")
                .Must(start => start > DateTime.UtcNow).WithMessage("Start date and time must be in the future.");

            RuleFor(c => c.EndsAtUtc)
                .GreaterThanOrEqualTo(c => c.StartsAtUtc).When(c => c.EndsAtUtc.HasValue)
                .WithMessage("End date and time must be greater than or equal to start date and time.");
        }
    }
}