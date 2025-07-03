using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Attendance.Domain.Events.DomainEvents
{
    public sealed record EventCreatedDomainEvent(
        Guid EventId,
        string Title,
        string Description,
        DateTime StartsAtUtc,
        DateTime? EndsAtUtc
        ) : DomainEvent;
}