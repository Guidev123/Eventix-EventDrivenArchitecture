using Eventix.Modules.Ticketing.Application.Orders.Dtos;
using Eventix.Modules.Ticketing.Application.Orders.UseCases.GetByCustomer;
using Eventix.Modules.Ticketing.Application.Orders.UseCases.GetById;
using Eventix.Modules.Ticketing.Domain.Orders.Entities;

namespace Eventix.Modules.Ticketing.Application.Orders.Mappers
{
    internal static class OrderMappers
    {
        public static GetOrderByIdResponse MapToGetOrderByIdResponse(this Order order)
            => new(order.Id, order.CustomerId, order.Status, order.CreatedAtUtc, order.OrderItems.MapToOrderItems(), order.TotalPrice?.Amount);

        public static GetOrdersByCustomerResponse MapToGetOrdersByCustomerResponse(this Order order)
            => new(order.Id, order.CustomerId, order.Status, order.CreatedAtUtc, order?.TotalPrice?.Amount);

        public static List<OrderItemResponse> MapToOrderItems(this IEnumerable<OrderItem> orderItems)
        {
            return orderItems.Select(x => new OrderItemResponse(
                x.Id, x.OrderId,
                x.TicketTypeId,
                x.Quantity.Value,
                x.UnitPrice.Amount,
                x.Price.Amount,
                x.Price.Currency
                )).ToList();
        }
    }
}