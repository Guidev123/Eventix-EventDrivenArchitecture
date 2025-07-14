using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Modules.Ticketing.Domain.Payments.DomainEvents;
using Eventix.Modules.Ticketing.Domain.Payments.Errors;
using Eventix.Modules.Ticketing.Domain.Payments.ValueObjects;
using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.Responses;
using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Ticketing.Domain.Payments.Entities
{
    public sealed class Payment : Entity, IAggregateRoot
    {
        private Payment(Guid orderId, Guid transactionId, decimal amount, string currency)

        {
            OrderId = orderId;
            TransactionId = transactionId;
            Amount = (amount, currency);
            DateInfo = new();
            Validate();
        }

        private Payment()
        { }

        public Guid OrderId { get; private set; }

        public Guid TransactionId { get; private set; }

        public Money Amount { get; private set; } = null!;

        public Money? AmountRefunded { get; private set; }

        public PaymentDateInfo DateInfo { get; private set; } = null!;

        public static Payment Create(Order order, Guid transactionId, decimal amount, string currency)
        {
            var payment = new Payment(order.Id, transactionId, amount, currency);

            payment.Raise(new PaymentCreatedDomainEvent(payment.Id));

            return payment;
        }

        public Result Refund(decimal refundAmount)
        {
            if (AmountRefunded is not null && AmountRefunded.Amount == Amount.Amount)
                return Result.Failure(PaymentErrors.AlreadyRefunded);

            if (AmountRefunded is not null
                && AmountRefunded.Amount + refundAmount > Amount.Amount)
                return Result.Failure(PaymentErrors.NotEnoughFunds);

            var amountRefunded = AmountRefunded is null ? decimal.Zero : AmountRefunded.Amount;
            var currencyAmountRefunded = AmountRefunded is null ? Amount.Currency : AmountRefunded.Currency;

            var currentAmountRefunded = amountRefunded += refundAmount;
            AmountRefunded = (currentAmountRefunded, currencyAmountRefunded);

            if (Amount.Amount == AmountRefunded?.Amount)
            {
                Raise(new PaymentRefundedDomainEvent(Id, TransactionId, refundAmount));
            }
            else
            {
                Raise(new PaymentPartiallyRefundedDomainEvent(Id, TransactionId, refundAmount));
            }

            return Result.Success();
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(
                OrderId.ToString(),
                PaymentErrors.InvalidOrderId.Description);

            AssertionConcern.EnsureNotEmpty(
                TransactionId.ToString(),
                PaymentErrors.InvalidTransactionId.Description);

            AssertionConcern.EnsureNotNull(
                Amount,
                PaymentErrors.AmountIsRequired.Description);

            AssertionConcern.EnsureNotNull(
                DateInfo,
                PaymentErrors.DateInfoIsRequired.Description);
        }
    }
}