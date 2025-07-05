using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Attendance.Domain.Events.Errors
{
    public static class EventErrors
    {
        public static Error NotFound(Guid eventId) =>
            Error.NotFound("Events.NotFound", $"The event with the identifier {eventId} was not found");

        public static Error FailToCancelEvent(Guid eventId) => Error.Problem(
            "Events.FailToCancelEvent",
            $"Something has failed to cancel the event with identifier {eventId}");

        public static readonly Error FailToCreateEvent = Error.Problem(
            "Events.FailToCreateEvent",
            "Something has failed to create event");

        public static readonly Error SpecificationIsRequired = Error.Problem(
           "Events.SpecificationIsRequired",
           "The event specification is required.");

        public static readonly Error TitleIsRequired = Error.Problem(
            "Events.TitleIsRequired",
            "The event title is required.");

        public static readonly Error TitleLengthInvalid = Error.Problem(
            "Events.TitleLengthInvalid",
            "The event title must be between 3 and 100 characters.");

        public static readonly Error InvalidEventId = Error.Problem(
            "Event.InvalidEventId",
            "Event ID cannot be empty");

        public static readonly Error LocationIsRequired = Error.Problem(
            "Event.LocationIsRequired",
            "Event location is required");

        public static readonly Error InvalidStartDate = Error.Problem(
            "Event.InvalidStartDate",
            "Event start date cannot be default value");

        public static readonly Error InvalidEndDate = Error.Problem(
            "Event.InvalidEndDate",
            "Event end date cannot be default value");

        public static readonly Error DescriptionIsRequired = Error.Problem(
            "Events.DescriptionIsRequired",
            "The event description is required.");

        public static readonly Error DescriptionLengthInvalid = Error.Problem(
            "Events.DescriptionLengthInvalid",
            "The event description must be between 10 and 1000 characters.");

        public static readonly Error DateRangeIsRequired = Error.Problem(
            "Events.DateRangeIsRequired",
            "The event date range is required.");

        public static readonly Error EndDateMustBeAfterStartDate = Error.Problem(
          "DateRange.EndDateMustBeAfterStartDate",
          "The end date must be after the start date.");

        public static readonly Error EventIdIsRequired = Error.Problem(
            "Events.EventIdIsRequired",
            "The event ID must not be empty or an empty GUID.");

        public static readonly Error StartDateMustBeInFuture = Error.Problem(
            "DateRange.StartDateMustBeInFuture",
            "The start date must be in the future.");

        public static readonly Error FailToRescheduleEvent = Error.Problem(
            "Events.FailToRescheduleEvent",
            "Something has failed to reschedule event");

        public static readonly Error StartDateInPast = Error.Problem(
            "Events.StartDateInPast",
            "The event start date is in the past");
    }
}