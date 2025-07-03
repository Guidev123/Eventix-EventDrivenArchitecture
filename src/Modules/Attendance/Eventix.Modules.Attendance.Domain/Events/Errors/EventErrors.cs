using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Attendance.Domain.Events.Errors
{
    public static class EventErrors
    {
        public static Error NotFound(Guid eventId) =>
            Error.NotFound("Events.NotFound", $"The event with the identifier {eventId} was not found");

        public static readonly Error SpecificationIsRequired = Error.Problem(
           "Events.SpecificationIsRequired",
           "The event specification is required.");

        public static readonly Error TitleIsRequired = Error.Problem(
            "Events.TitleIsRequired",
            "The event title is required.");

        public static readonly Error TitleLengthInvalid = Error.Problem(
            "Events.TitleLengthInvalid",
            "The event title must be between 3 and 100 characters.");

        public static readonly Error DescriptionIsRequired = Error.Problem(
            "Events.DescriptionIsRequired",
            "The event description is required.");

        public static readonly Error DescriptionLengthInvalid = Error.Problem(
            "Events.DescriptionLengthInvalid",
            "The event description must be between 10 and 1000 characters.");

        public static readonly Error DateRangeIsRequired = Error.Problem(
            "Events.DateRangeIsRequired",
            "The event date range is required.");
    }
}