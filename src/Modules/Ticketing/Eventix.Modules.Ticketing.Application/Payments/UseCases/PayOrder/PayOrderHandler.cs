using Eventix.Modules.Ticketing.Application.Abstractions.Services;
using Eventix.Modules.Ticketing.Domain.Orders.Errors;
using Eventix.Modules.Ticketing.Domain.Orders.Interfaces;
using Eventix.Modules.Ticketing.Domain.Payments.Entities;
using Eventix.Modules.Ticketing.Domain.Payments.Errors;
using Eventix.Modules.Ticketing.Domain.Payments.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Payments.UseCases.PayOrder
{
    internal sealed class PayOrderHandler(IPaymentRepository paymentRepository,
                                          IOrderRepository orderRepository,
                                          IPaymentService paymentService) : ICommandHandler<PayOrderCommand>
    {
        public async Task<Result> ExecuteAsync(PayOrderCommand request, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order is null)
                return Result.Failure(OrderErrors.NotFound(request.OrderId));

            if (order.TotalPrice is null)
                return Result.Failure(OrderErrors.TotalPriceMustBeNotNull);

            var paymentResponse = await paymentService.ChargeAsync(order.TotalPrice.Amount, order.TotalPrice.Currency);

            if (paymentResponse.IsFailure)
                return Result.Failure(PaymentErrors.FailToCreatePayment);

            var payment = Payment.Create(
                order,
                paymentResponse.Value.TransactionId,
                order.TotalPrice.Amount,
                order.TotalPrice.Currency);

            paymentRepository.Insert(payment);

            var saveChanges = await paymentRepository.UnitOfWork.CommitAsync(cancellationToken);

            return saveChanges
                ? Result.Success()
                : Result.Failure(PaymentErrors.FailToPayOrder(request.OrderId));
        }
    }
}