using Eventix.Modules.Attendance.Domain.Attendees.Errors;
using Eventix.Shared.Domain.ValueObjects;
using FluentValidation;

namespace Eventix.Modules.Attendance.Application.Attendees.UseCases.Update
{
    internal sealed class UpdateAttendeeValidator : AbstractValidator<UpdateAttendeeCommand>
    {
        public UpdateAttendeeValidator()
        {
            RuleFor(x => x.AttendeeId)
                .NotEmpty()
                .WithMessage(AttendeeErrors.AttendeeIdIsRequired.Description);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(Name.NAME_MAX_LENGTH)
                .WithMessage(AttendeeErrors.FirstNameLengthInvalid.Description);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(Name.NAME_MAX_LENGTH)
                .WithMessage(AttendeeErrors.FirstNameLengthInvalid.Description);
        }
    }
}