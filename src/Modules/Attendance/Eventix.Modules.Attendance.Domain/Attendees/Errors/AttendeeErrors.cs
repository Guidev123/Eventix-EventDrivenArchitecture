using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Attendance.Domain.Attendees.Errors
{
    public static class AttendeeErrors
    {
        public static Error NotFound(Guid attendeeId) =>
            Error.NotFound("Attendees.NotFound", $"The attendee with the identifier {attendeeId} was not found");

        public static readonly Error EmailMustBeNotEmpty = Error.Problem(
            "Attendee.EmailMustBeNotEmpty",
            "E-mail can not be empty");

        public static readonly Error NameMustBeNotEmpty = Error.Problem(
            "Attendee.NameMustBeNotEmpty",
            "Name can not be empty");
    }
}