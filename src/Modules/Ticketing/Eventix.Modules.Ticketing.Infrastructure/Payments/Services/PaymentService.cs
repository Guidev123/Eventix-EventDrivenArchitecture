using Eventix.Modules.Ticketing.Application.Abstractions.Services;
using Eventix.Modules.Ticketing.Application.Orders.Dtos;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Infrastructure.Payments.Services
{
    internal sealed class PaymentService : IPaymentService
    {
        public async Task<Result<PaymentDto>> ChargeAsync(decimal amount, string currency)
        {
            // TODO: Integration with stripe
            await Task.Delay(500);
            return Result.Success(new PaymentDto(Guid.NewGuid(), decimal.Zero, string.Empty));
        }

        public async Task RefundAsync(Guid transactionId, decimal amount)
        {
            // TODO: Integration with stripe
            await Task.Delay(500);
        }
    }
}