using Eventix.Modules.Attendance.Domain.Attendees.Errors;
using Eventix.Modules.Attendance.Domain.Events.Errors;
using Eventix.Modules.Attendance.Domain.Tickets.Errors;
using FluentValidation;

namespace Eventix.Modules.Attendance.Application.Tickets.UseCases.Create
{
    internal sealed class CreateTicketValidator : AbstractValidator<CreateTicketCommand>
    {
        public CreateTicketValidator()
        {
            RuleFor(c => c.AttendeeId)
                .NotEmpty()
                .WithMessage(AttendeeErrors.AttendeeIdIsRequired.Description);

            RuleFor(c => c.EventId)
                .NotEmpty()
                .WithMessage(EventErrors.EventIdIsRequired.Description);

            RuleFor(c => c.TicketId)
                .NotEmpty()
                .WithMessage(TicketErrors.TicketIdMustBeNotEmpty.Description);

            RuleFor(c => c.Code)
                .NotEmpty()
                .WithMessage(TicketErrors.CodeIsRequired.Description);
        }
    }
}