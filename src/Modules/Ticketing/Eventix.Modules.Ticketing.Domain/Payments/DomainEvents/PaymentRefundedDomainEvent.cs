using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Ticketing.Domain.Payments.DomainEvents
{
    public sealed record PaymentRefundedDomainEvent(Guid PaymentId, Guid TransactionId, decimal RefundAmount)
    : DomainEvent(PaymentId);
}