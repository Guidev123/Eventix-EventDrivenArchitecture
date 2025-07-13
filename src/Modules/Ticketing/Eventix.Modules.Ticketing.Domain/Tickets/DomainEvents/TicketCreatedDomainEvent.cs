using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Ticketing.Domain.Tickets.DomainEvents
{
    public sealed record TicketCreatedDomainEvent(Guid TicketId) : DomainEvent(TicketId);
}