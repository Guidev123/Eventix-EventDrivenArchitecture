using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Events.Domain.Events.DomainEvents
{
    public record EventRescheduledDomainEvent : DomainEvent
    {
        public EventRescheduledDomainEvent(Guid eventId, DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            EventId = eventId;
            StartsAtUtc = startsAtUtc;
            EndsAtUtc = endsAtUtc;
        }

        public Guid EventId { get; }
        public DateTime StartsAtUtc { get; }
        public DateTime? EndsAtUtc { get; }
    }
}