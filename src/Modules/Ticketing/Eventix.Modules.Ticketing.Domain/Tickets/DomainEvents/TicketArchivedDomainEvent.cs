using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Ticketing.Domain.Tickets.DomainEvents
{
    public sealed record TicketArchivedDomainEvent(Guid TicketId, string Code) : DomainEvent(TicketId);
}