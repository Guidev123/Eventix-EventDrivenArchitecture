using Eventix.Modules.Events.Domain.TicketTypes.ValueObjects;
using FluentValidation;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.Create
{
    public sealed class CreateTicketTypeValidator : AbstractValidator<CreateTicketTypeCommand>
    {
        public CreateTicketTypeValidator()
        {
            RuleFor(c => c.EventId).NotEmpty().WithMessage("Event ID must be not empty.");

            RuleFor(c => c.Name).NotEmpty().WithMessage("Ticket type name must be not empty.")
                .MaximumLength(100).WithMessage("Ticket type name must not exceed 100 characters.");

            RuleFor(c => c.Price).GreaterThan(0).WithMessage("Ticket type price must be greater than zero.");

            RuleFor(c => c.Currency)
                .MinimumLength(Money.MIN_CURRENCY_LENGTH)
                .WithMessage($"Currency length must be greater than {Money.MIN_CURRENCY_LENGTH} caracters.")
                .MaximumLength(Money.MAX_CURRENCY_LENGTH)
                .WithMessage($"Currency length must be less than {Money.MAX_CURRENCY_LENGTH} caracters.")
                .NotEmpty()
                .WithMessage("Currency must be not empty.");

            RuleFor(c => c.Quantity).GreaterThan(0).WithMessage("Ticket type quantity must be greater than zero.");
        }
    }
}