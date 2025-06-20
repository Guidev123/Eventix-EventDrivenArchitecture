using Eventix.Modules.Ticketing.Domain.Payments.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Ticketing.Domain.Payments.ValueObjects
{
    public sealed record PaymentDateInfo : ValueObject
    {
        public PaymentDateInfo(DateTime? refundedAtUtc = null)
        {
            CreatedAtUtc = DateTime.UtcNow;
            RefundedAtUtc = refundedAtUtc;
            Validate();
        }

        private PaymentDateInfo()
        { }

        public DateTime CreatedAtUtc { get; }

        public DateTime? RefundedAtUtc { get; }

        protected override void Validate()
        {
            AssertionConcern.EnsureFalse(
                CreatedAtUtc == default,
                PaymentErrors.InvalidCreatedDate.Description);

            if (RefundedAtUtc.HasValue)
            {
                AssertionConcern.EnsureFalse(
                    RefundedAtUtc.Value == default,
                    PaymentErrors.InvalidRefundDate.Description);

                AssertionConcern.EnsureTrue(
                    RefundedAtUtc.Value >= CreatedAtUtc,
                    PaymentErrors.RefundDateMustBeAfterCreation.Description);
            }
        }
    }
}