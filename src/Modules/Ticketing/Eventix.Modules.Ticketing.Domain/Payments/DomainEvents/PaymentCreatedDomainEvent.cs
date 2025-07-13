using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Ticketing.Domain.Payments.DomainEvents
{
    public sealed record PaymentCreatedDomainEvent(Guid PaymentId) : DomainEvent(PaymentId);
}