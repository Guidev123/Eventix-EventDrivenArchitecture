using Eventix.Modules.Ticketing.Application.Orders.Dtos;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Abstractions.Services
{
    public interface IPaymentService
    {
        Task<Result<PaymentResponse>> ChargeAsync(decimal amount, string currency);

        Task RefundAsync(Guid transactionId, decimal amount);
    }
}