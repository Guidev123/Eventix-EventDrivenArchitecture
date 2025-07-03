using Eventix.Modules.Ticketing.Domain.Tickets.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.ArchiveForEvent
{
    internal sealed class ArchiveTicketsForEventValidator : AbstractValidator<ArchiveTicketsForEventCommand>
    {
        public ArchiveTicketsForEventValidator()
        {
            RuleFor(a => a.EventId).NotEmpty().WithMessage(TicketErrors.InvalidEventId.Description);
        }
    }
}