using Eventix.Modules.Ticketing.Application.Orders.Dtos;
using Eventix.Modules.Ticketing.Domain.Orders.Enumerators;
using System.Text.Json.Serialization;

namespace Eventix.Modules.Ticketing.Application.Orders.UseCases.GetById
{
    public sealed record GetOrderByIdResponse
    {
        public GetOrderByIdResponse(
            Guid customerId,
            Guid orderId,
            OrderStatusEnum status,
            DateTime createdAtUtc,
            List<OrderItemResponse> orderItems,
            decimal? totalPrice = null
            )
        {
            CustomerId = customerId;
            Status = status;
            TotalPrice = totalPrice;
            CreatedAtUtc = createdAtUtc;
            OrderId = orderId;
            OrderItems = orderItems;
        }
        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatusEnum Status { get; }
        public decimal? TotalPrice { get; }
        public DateTime CreatedAtUtc { get; }
        public List<OrderItemResponse> OrderItems { get; set; }
    }
}