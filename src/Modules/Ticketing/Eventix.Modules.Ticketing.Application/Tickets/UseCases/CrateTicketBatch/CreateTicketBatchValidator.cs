using Eventix.Modules.Ticketing.Domain.Tickets.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.CrateTicketBatch
{
    public sealed class CreateTicketBatchValidator : AbstractValidator<CreateTicketBatchCommand>
    {
        public CreateTicketBatchValidator()
        {
            RuleFor(tb => tb.OrderId).NotEmpty().WithMessage(TicketErrors.InvalidOrderId.Description);
        }
    }
}