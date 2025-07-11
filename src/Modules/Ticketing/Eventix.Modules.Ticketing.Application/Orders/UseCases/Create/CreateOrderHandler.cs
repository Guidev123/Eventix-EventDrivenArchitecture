using Eventix.Modules.Ticketing.Application.Carts.Errors;
using Eventix.Modules.Ticketing.Application.Carts.Models;
using Eventix.Modules.Ticketing.Application.Carts.Services;
using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;
using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.Domain.Events.Interfaces;
using Eventix.Modules.Ticketing.Domain.Orders.DomainEvents;
using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.Errors;
using Eventix.Modules.Ticketing.Domain.Orders.Interfaces;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;
using System.Data.Common;

namespace Eventix.Modules.Ticketing.Application.Orders.UseCases.Create
{
    internal sealed class CreateOrderHandler(ICustomerRepository customerRepository,
                                             IOrderRepository orderRepository,
                                             ITicketTypeRepository ticketTypeRepository,
                                             ISqlConnectionFactory sqlConnectionFactory,
                                             ICartService cartService) : ICommandHandler<CreateOrderCommand>
    {
        public async Task<Result> ExecuteAsync(CreateOrderCommand request, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();
            await connection.OpenAsync(cancellationToken);

            using var transaction = await connection.BeginTransactionAsync(cancellationToken);

            try
            {
                var orderResultTask = CreateOrderAsync(request, cancellationToken);
                var cartResultTask = GetCartAsync(request.CustomerId, cancellationToken);

                await Task.WhenAll(orderResultTask, cartResultTask);

                var orderResult = orderResultTask.Result;
                var cartResult = cartResultTask.Result;

                if (orderResult.IsFailure)
                    return orderResult;

                if (cartResult.IsFailure)
                    return cartResult;

                var order = orderResult.Value;

                var getCartItemsResult = await GetCartItemsAsync(cartResult.Value, order, cancellationToken);
                if (getCartItemsResult.IsFailure)
                    return getCartItemsResult;

                return await PersistDataAsync(order, transaction, cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(OrderErrors.UnableToPersistOrder);
            }
        }

        private async Task<Result> PersistDataAsync(Order order, DbTransaction transaction, CancellationToken cancellationToken)
        {
            if (order.TotalPrice is null)
                return Result.Failure(OrderErrors.TotalPriceMustBeNotNull);

            orderRepository.Insert(order);

            order.Raise(new OrderPlacedDomainEvent(order.Id, order.TotalPrice.Amount, order.TotalPrice.Currency));

            var saveChangesOrder = await orderRepository.UnitOfWork.CommitAsync(cancellationToken);

            if (!saveChangesOrder)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(OrderErrors.FailToCreateOrder);
            }

            await transaction.CommitAsync(cancellationToken);

            await cartService.ClearAsync(order.CustomerId, cancellationToken);

            return Result.Success();
        }

        private async Task<Result<Order>> CreateOrderAsync(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer is null)
                return Result.Failure<Order>(CustomerErrors.NotFound(request.CustomerId));

            var order = Order.Create(customer);

            return Result.Success(order);
        }

        private async Task<Result<Cart>> GetCartAsync(Guid customerId, CancellationToken cancellationToken)
        {
            var cart = await cartService.GetAsync(customerId, cancellationToken);
            if (cart.IsFailure)
                return Result.Failure<Cart>(CartErrors.NotFound(customerId));

            if (cart.Value.Items.Count == 0)
                return Result.Failure<Cart>(CartErrors.Empty);

            return Result.Success(cart.Value);
        }

        private async Task<Result> GetCartItemsAsync(Cart cart, Order order, CancellationToken cancellationToken)
        {
            foreach (CartItem cartItem in cart.Items)
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

            return Result.Success();
        }
    }
}