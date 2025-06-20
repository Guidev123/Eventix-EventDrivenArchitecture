using Eventix.Modules.Ticketing.Application.Orders.Dtos;

namespace Eventix.Modules.Ticketing.Application.Abstractions.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto> ChargeAsync(decimal amount, string currency);

        Task RefundAsync(Guid transactionId, decimal amount);
    }
}