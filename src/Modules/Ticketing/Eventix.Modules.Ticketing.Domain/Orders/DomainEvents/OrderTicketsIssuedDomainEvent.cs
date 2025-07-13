using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Ticketing.Domain.Orders.DomainEvents
{
    public sealed record OrderTicketsIssuedDomainEvent(Guid OrderId) : DomainEvent(OrderId);
}