using Eventix.Modules.Attendance.Domain.Attendees.Errors;
using Eventix.Shared.Domain.ValueObjects;
using FluentValidation;

namespace Eventix.Modules.Attendance.Application.Attendees.UseCases.Create
{
    internal sealed class CreateAttendeeValidator : AbstractValidator<CreateAttendeeCommand>
    {
        public CreateAttendeeValidator()
        {
            RuleFor(x => x.AttendeeId)
                .NotEmpty()
                .WithMessage(AttendeeErrors.AttendeeIdIsRequired.Description);

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage(AttendeeErrors.InvalidEmailFormat.Description);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(Name.NAME_MIN_LENGTH, Name.NAME_MAX_LENGTH)
                .WithMessage(AttendeeErrors.FirstNameLengthInvalid.Description);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(Name.NAME_MIN_LENGTH, Name.NAME_MAX_LENGTH)
                .WithMessage(AttendeeErrors.LastNameLengthInvalid.Description);
        }
    }
}