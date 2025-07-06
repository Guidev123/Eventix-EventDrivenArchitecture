using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Domain.Events.Errors
{
    public static class EventErrors
    {
        public static Error NotFound(Guid eventId) =>
                Error.NotFound("Events.NotFound", $"The event with the identifier {eventId} was not found");

        public static readonly Error PriceMustBeGreaterThanZero = Error.Problem(
            "Money.PriceMustBeGreaterThanZero",
            "The ticket price must be greater than zero.");

        public static readonly Error InvalidEndDate = Error.Problem(
            "Event.InvalidEndDate",
            "Event end date cannot be default value");

        public static readonly Error LocationNotSet = Error.Problem(
            "Events.LocationNotSet",
            "The event location must be set before publishing the event.");

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

        public static readonly Error UnableToAttachLocation = Error.Problem(
            "Events.UnableToAttachLocation",
            "Unable to attach the location to the event. Please try again later.");

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

        public static readonly Error CategoryIdIsRequired = Error.Problem(
            "Events.CategoryIdIsRequired",
            "The event category identifier is required.");

        public static readonly Error StartDateIsRequired = Error.Problem(
            "Events.StartDateIsRequired",
            "The event start date is required.");

        public static readonly Error DateRangeIsRequired = Error.Problem(
            "Events.DateRangeIsRequired",
            "The event date range is required.");

        public static readonly Error EndDateMustBeAfterStartDate = Error.Problem(
            "DateRange.EndDateMustBeAfterStartDate",
            "The end date must be after the start date.");

        public static readonly Error EventIdIsRequired = Error.Problem(
            "Events.EventIdIsRequired",
            "The event ID must not be empty or an empty GUID.");

        public static Error TitleTooShort(int minLength) =>
       Error.Problem("Events.TitleTooShort", $"The event title must be at least {minLength} characters long.");

        public static Error TitleTooLong(int maxLength) =>
            Error.Problem("Events.TitleTooLong", $"The event title must not exceed {maxLength} characters.");

        public static Error DescriptionTooShort(int minLength) =>
            Error.Problem("Events.DescriptionTooShort", $"The event description must be at least {minLength} characters long.");

        public static Error DescriptionTooLong(int maxLength) =>
            Error.Problem("Events.DescriptionTooLong", $"The event description must not exceed {maxLength} characters.");

        public static Error StartDateTooSoon(int minimumStartTime) =>
            Error.Problem("Events.StartDateTooSoon", $"The event must start at least {minimumStartTime} hour(s) later.");

        public static readonly Error EndDateBeforeStartDate = Error.Problem(
            "Events.EndDateBeforeStartDate",
            "The event end date must be after the start date.");
    }
}