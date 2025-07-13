using Eventix.Shared.Application.EventBus;

namespace Eventix.Modules.Events.IntegrationEvents.Events
{
    public sealed record EventPublishedIntegrationEvent : IntegrationEvent
    {
        public EventPublishedIntegrationEvent(
            Guid correlationId,
            DateTime occurredOnUtc,
            Guid eventId,
            string title,
            string description,
            Guid categoryId,
            DateTime startsAtUtc,
            DateTime? endsAtUtc,
            LocationRequest? location,
            List<TicketTypeRequest> ticketTypes
            ) : base(correlationId, occurredOnUtc)
        {
            EventId = eventId;
            Title = title;
            Description = description;
            CategoryId = categoryId;
            StartsAtUtc = startsAtUtc;
            EndsAtUtc = endsAtUtc;
            Location = location;
            TicketTypes = ticketTypes;
        }

        public Guid EventId { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public Guid CategoryId { get; init; }
        public DateTime StartsAtUtc { get; init; }
        public DateTime? EndsAtUtc { get; init; }
        public LocationRequest? Location { get; init; }
        public List<TicketTypeRequest> TicketTypes { get; init; } = [];

        public sealed record TicketTypeRequest(
            Guid TicketTypeId,
            Guid EventId,
            string Name,
            decimal Price,
            string Currency,
            decimal Quantity);

        public sealed record LocationRequest(
            string Street,
            string Number,
            string AdditionalInfo,
            string Neighborhood,
            string ZipCode,
            string City,
            string State
            );
    }
}