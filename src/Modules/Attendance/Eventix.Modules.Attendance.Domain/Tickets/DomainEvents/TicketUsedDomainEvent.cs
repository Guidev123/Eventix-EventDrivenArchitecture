using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Attendance.Domain.Tickets.DomainEvents
{
    public sealed record TicketUsedDomainEvent(
        Guid TicketId
        ) : DomainEvent;
}