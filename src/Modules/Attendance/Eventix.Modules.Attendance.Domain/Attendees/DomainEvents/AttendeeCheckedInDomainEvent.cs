using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Attendance.Domain.Attendees.DomainEvents
{
    public sealed record AttendeeCheckedInDomainEvent(
        Guid AttendeeId,
        Guid EventId
        ) : DomainEvent;
}