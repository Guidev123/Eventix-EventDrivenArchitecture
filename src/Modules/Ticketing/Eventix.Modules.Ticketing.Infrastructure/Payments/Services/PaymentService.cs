using Eventix.Modules.Ticketing.Application.Abstractions.Services;
using Eventix.Modules.Ticketing.Application.Orders.Dtos;

namespace Eventix.Modules.Ticketing.Infrastructure.Payments.Services
{
    internal sealed class PaymentService : IPaymentService
    {
        public Task<PaymentDto> ChargeAsync(decimal amount, string currency)
        {
            // TODO: Integration with stripe
            return Task.FromResult(new PaymentDto(Guid.NewGuid(), decimal.Zero, string.Empty));
        }

        public async Task RefundAsync(Guid transactionId, decimal amount)
        {
            // TODO: Integration with stripe
            await Task.Delay(500);
        }
    }
}