using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Ticketing.Domain.Payments.DomainEvents
{
    public sealed record PaymentPartiallyRefundedDomainEvent(Guid PaymentId, Guid TransactionId, decimal RefundAmount)
    : DomainEvent;
}