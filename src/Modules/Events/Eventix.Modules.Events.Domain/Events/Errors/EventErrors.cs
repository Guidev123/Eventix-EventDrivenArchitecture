using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Domain.Events.Errors
{
    public static class EventErrors
    {
        public static Error NotFound(Guid eventId) =>
                Error.NotFound("Events.NotFound", $"The event with the identifier {eventId} was not found");

        public static Error UnableToCancelEvent(Guid eventId) =>
            Error.Problem("Events.UnableToCancelEvent", $"Unable to cancel the event with identifier {eventId}");

        public static readonly Error EventCanNotBePublished = Error.Problem(
            "Events.EventCanNotBePublished",
            "The event can not be published.");

        public static readonly Error StartDateInPast = Error.Problem(
            "Events.StartDateInPast",
            "The event start date is in the past");

        public static readonly Error EndDatePrecedesStartDate = Error.Problem(
            "Events.EndDatePrecedesStartDate",
            "The event end date precedes the start date");

        public static readonly Error NoTicketsFound = Error.Problem(
            "Events.NoTicketsFound",
            "The event does not have any ticket types defined");

        public static readonly Error NotDraft = Error.Problem("Events.NotDraft", "The event is not in draft status");

        public static readonly Error AlreadyCanceled = Error.Problem(
            "Events.AlreadyCanceled",
            "The event was already canceled");

        public static readonly Error AlreadyStarted = Error.Problem(
            "Events.AlreadyStarted",
            "The event has already started");

        public static readonly Error UnableToCreateEvent = Error.Problem(
            "Events.UnableToCreateEvent",
            "Unable to create the event. Please try again later.");
    }
}