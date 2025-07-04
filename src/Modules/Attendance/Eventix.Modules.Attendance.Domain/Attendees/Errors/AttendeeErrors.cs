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

        public static readonly Error AttendeeIdIsRequired = Error.Problem(
           "Customers.CustomerIdIsRequired",
           "Customer ID must not be empty");

        public static readonly Error InvalidEmailFormat = Error.Problem(
            "Customers.InvalidEmailFormat",
            "Invalid e-mail format");

        public static readonly Error FirstNameLengthInvalid = Error.Problem(
            "Customers.FirstNameLengthInvalid",
            "First Name must be between 2 and 50 characters");

        public static readonly Error LastNameLengthInvalid = Error.Problem(
            "Customers.LastNameLengthInvalid",
            "Last Name must be between 2 and 50 characters");

        public static readonly Error SomethingHasFailedDuringPersistence = Error.Problem(
            "Attendees.SomethingHasFailedDuringPersistence",
            "Something has failed during persistence of the attendee");
    }
}