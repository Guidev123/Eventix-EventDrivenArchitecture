using FluentValidation;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetById
{
    public sealed class GetTicketTypeByIdValidator : AbstractValidator<GetTicketTypeByIdQuery>
    {
        public GetTicketTypeByIdValidator()
        {
            RuleFor(x => x.TicketTypeId).NotEmpty().WithMessage("Ticket Type ID must not be empty.");
        }
    }
}