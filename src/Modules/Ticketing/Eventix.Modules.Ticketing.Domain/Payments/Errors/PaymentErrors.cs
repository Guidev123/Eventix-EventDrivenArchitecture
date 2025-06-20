using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Domain.Payments.Errors
{
    public static class PaymentErrors
    {
        public static Error NotFound(Guid paymentId) =>
            Error.NotFound("Payments.NotFound", $"The payment with the identifier {paymentId} was not found");

        public static readonly Error AlreadyRefunded =
            Error.Problem("Payments.AlreadyRefunded", "The payment was already refunded");

        public static readonly Error NotEnoughFunds =
            Error.Problem("Payments.NotEnoughFunds", "There are not enough funds for a refund");

        public static readonly Error FailToPersistRefundInformation = Error.Problem(
            "Payments.FailToPersistRefundInformation",
            "Something has failed to persist refund information");

        public static readonly Error FailToCreatePayment = Error.Problem(
            "Payment.FailToCreatePayment",
            "Something has failed during payment creation");

        public static readonly Error InvalidPaymentData = Error.Problem(
            "Payment.InvalidPaymentData",
            "Invalid payment data provided");

        public static readonly Error InvalidOrderId = Error.Problem(
            "Payment.InvalidOrderId",
            "Order ID cannot be empty");

        public static readonly Error InvalidEventId = Error.Problem(
            "Payment.InvalidEventId",
            "Event ID cannot be empty");

        public static readonly Error InvalidTransactionId = Error.Problem(
            "Payment.InvalidTransactionId",
            "Transaction ID cannot be empty");

        public static readonly Error AmountIsRequired = Error.Problem(
            "Payment.AmountIsRequired",
            "Payment amount is required");

        public static readonly Error DateInfoIsRequired = Error.Problem(
            "Payment.DateInfoIsRequired",
            "Payment date information is required");

        public static readonly Error InvalidCreatedDate = Error.Problem(
            "PaymentDateInfo.InvalidCreatedDate",
            "Created date cannot be default value");

        public static readonly Error InvalidRefundDate = Error.Problem(
            "PaymentDateInfo.InvalidRefundDate",
            "Refund date cannot be default value");

        public static readonly Error RefundDateMustBeAfterCreation = Error.Problem(
            "PaymentDateInfo.RefundDateMustBeAfterCreation",
            "Refund date must be after or equal to creation date");
    }
}