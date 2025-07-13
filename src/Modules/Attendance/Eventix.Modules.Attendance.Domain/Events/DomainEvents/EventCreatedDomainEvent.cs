using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Attendance.Domain.Events.DomainEvents
{
    public sealed record EventCreatedDomainEvent(
        Guid EventId
        ) : DomainEvent(EventId);
}