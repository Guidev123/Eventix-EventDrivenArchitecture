using FluentValidation;

namespace Eventix.Modules.Events.Application.TicketTypes.Create
{
    public sealed class CreateTicketTypeValidator : AbstractValidator<CreateTicketTypeCommand>
    {
        public CreateTicketTypeValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("CreateTicketTypeCommand cannot be null.");
        }
    }
}