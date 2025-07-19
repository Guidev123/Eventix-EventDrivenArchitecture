using Eventix.Modules.Ticketing.Application.Abstractions.Services;
using Eventix.Modules.Ticketing.Application.Orders.Dtos;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Infrastructure.Payments.Services
{
    internal sealed class PaymentService : IPaymentService
    {
        public async Task<Result<PaymentResponse>> ChargeAsync(decimal amount, string currency)
        {
            // Mock Integration with payment gateway
            await Task.Delay(500);

            return Result.Success(new PaymentResponse(Guid.NewGuid(), amount, currency));
        }

        public async Task RefundAsync(Guid transactionId, decimal amount)
        {
            // Mock Integration with payment gateway
            await Task.Delay(500);
        }
    }
}