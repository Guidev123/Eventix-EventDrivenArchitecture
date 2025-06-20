using Eventix.Modules.Ticketing.Application.Abstractions.Services;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Payments.UseCases.Refund
{
    public record RefundPaymentCommand(Guid PaymentId, decimal Amount) : ICommand;
}