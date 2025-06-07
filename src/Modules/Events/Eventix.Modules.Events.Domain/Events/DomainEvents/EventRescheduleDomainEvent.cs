using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Events.Domain.Events.DomainEvents
{
    public record EventRescheduleDomainEvent : DomainEvent
    {
        public EventRescheduleDomainEvent(Guid eventId, DateTime startsAtUtc, DateTime? endsAtUtc)
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