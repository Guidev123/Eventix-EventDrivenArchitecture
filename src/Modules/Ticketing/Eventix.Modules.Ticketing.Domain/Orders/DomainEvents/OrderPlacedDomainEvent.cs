using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Ticketing.Domain.Orders.DomainEvents
{
    public sealed record OrderPlacedDomainEvent(
        Guid OrderId,
        decimal TotalPrice,
        string Currency
        ) : DomainEvent;
}