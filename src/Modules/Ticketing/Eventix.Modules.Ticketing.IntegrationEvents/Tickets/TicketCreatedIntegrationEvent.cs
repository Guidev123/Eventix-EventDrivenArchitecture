using Eventix.Shared.Application.EventBus;

namespace Eventix.Modules.Ticketing.IntegrationEvents.Tickets
{
    public sealed record TicketCreatedIntegrationEvent : IntegrationEvent
    {
        public TicketCreatedIntegrationEvent(
            Guid id,
            DateTime occurredOnUtc,
            Guid ticketId,
            Guid attendeeId,
            Guid eventId,
            string code
            ) : base(id, occurredOnUtc)
        {
            TicketId = ticketId;
            AttendeeId = attendeeId;
            EventId = eventId;
            Code = code;
        }

        public Guid TicketId { get; init; }
        public Guid AttendeeId { get; init; }
        public Guid EventId { get; init; }
        public string Code { get; init; }
    }
}