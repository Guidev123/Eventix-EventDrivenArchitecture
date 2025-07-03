using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Payments.UseCases.Refund
{
    public sealed record RefundPaymentCommand(Guid PaymentId, decimal Amount) : ICommand;
}