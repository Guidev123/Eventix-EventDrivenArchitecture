using Eventix.Modules.Ticketing.Application.Abstractions.Services;
using Eventix.Modules.Ticketing.Application.Carts.Errors;
using Eventix.Modules.Ticketing.Application.Carts.Models;
using Eventix.Modules.Ticketing.Application.Carts.Services;
using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;
using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.Domain.Events.Interfaces;
using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.Errors;
using Eventix.Modules.Ticketing.Domain.Orders.Interfaces;
using Eventix.Modules.Ticketing.Domain.Payments.Entities;
using Eventix.Modules.Ticketing.Domain.Payments.Errors;
using Eventix.Modules.Ticketing.Domain.Payments.Interfaces;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Orders.UseCases.Create
{
    internal sealed class CreateOrderHandler(ICustomerRepository customerRepository,
                                             IOrderRepository orderRepository,
                                             ITicketTypeRepository ticketTypeRepository,
                                             IPaymentRepository paymentRepository,
                                             IPaymentService paymentService,
                                             ISqlConnectionFactory sqlConnectionFactory,
                                             ICartService cartService) : ICommandHandler<CreateOrderCommand>
    {
        public async Task<Result> ExecuteAsync(CreateOrderCommand request, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();
            using var transaction = await connection.BeginTransactionAsync(cancellationToken);

            try
            {
                var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
                if (customer is null)
                    return Result.Failure(CustomerErrors.NotFound(request.CustomerId));

                var order = Order.Create(customer);

                var cart = await cartService.GetAsync(customer.Id, cancellationToken);
                if (cart.IsFailure)
                    return Result.Failure(CartErrors.NotFound(request.CustomerId));

                if (cart.Value.Items.Count == 0)
                    return Result.Failure(CartErrors.Empty);

                foreach (CartItem cartItem in cart.Value.Items)
                {
                    var ticketType = await ticketTypeRepository.GetWithLockAsync(
                        cartItem.TicketTypeId,
                        cancellationToken);

                    if (ticketType is null)
                        return Result.Failure(TicketTypeErrors.NotFound(cartItem.TicketTypeId));

                    var result = ticketType.UpdateQuantity(cartItem.Quantity);

                    if (result.IsFailure)
                        return Result.Failure(result.Error!);

                    order.AddItem(ticketType, cartItem.Quantity, cartItem.Amount, ticketType.Price.Currency);
                }

                orderRepository.Insert(order);

                if (order.TotalPrice is null)
                    return Result.Failure(OrderErrors.TotalPriceMustBeNotNull);

                var paymentResponse = await paymentService.ChargeAsync(order.TotalPrice.Amount, order.TotalPrice.Currency);

                if (paymentResponse.IsFailure)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result.Failure(PaymentErrors.FailToCreatePayment);
                }

                var payment = Payment.Create(
                    order,
                    paymentResponse.Value.TransactionId,
                    paymentResponse.Value.Amount,
                    paymentResponse.Value.Currency);

                paymentRepository.Insert(payment);

                var saveChangesPayment = await paymentRepository.UnitOfWork.CommitAsync(cancellationToken);
                var saveChangesOrder = await orderRepository.UnitOfWork.CommitAsync(cancellationToken);

                if (!saveChangesPayment || !saveChangesOrder)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result.Failure(OrderErrors.FailToCreateOrder);
                }

                await transaction.CommitAsync(cancellationToken);

                await cartService.ClearAsync(customer.Id, cancellationToken);

                return Result.Success();
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(OrderErrors.UnableToPersistOrder);
            }
        }
    }
}