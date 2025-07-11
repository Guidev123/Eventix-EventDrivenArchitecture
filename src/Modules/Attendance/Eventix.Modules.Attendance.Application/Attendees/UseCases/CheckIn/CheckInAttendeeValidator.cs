using Eventix.Modules.Attendance.Domain.Attendees.Errors;
using FluentValidation;

namespace Eventix.Modules.Attendance.Application.Attendees.UseCases.CheckIn
{
    internal sealed class CheckInAttendeeValidator : AbstractValidator<CheckInAttendeeCommand>
    {
        public CheckInAttendeeValidator()
        {
            RuleFor(c => c.AttendeeId)
                .NotEmpty()
                .WithMessage(AttendeeErrors.AttendeeIdIsRequired.Description);

            RuleFor(c => c.TicketId)
                .NotEmpty()
                .WithMessage(AttendeeErrors.TicketIdMustBeNotEmpty.Description);
        }
    }
}