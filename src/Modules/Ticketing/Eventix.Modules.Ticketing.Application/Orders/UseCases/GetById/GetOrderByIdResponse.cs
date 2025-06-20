using Eventix.Modules.Ticketing.Domain.Orders.Enumerators;
using System.Text.Json.Serialization;

namespace Eventix.Modules.Ticketing.Application.Orders.UseCases.GetById
{
    public record GetOrderByIdResponse
    {
        public GetOrderByIdResponse(Guid customerId, OrderStatusEnum status, DateTime createdAtUtc, decimal? totalPrice = null)
        {
            CustomerId = customerId;
            Status = status;
            TotalPrice = totalPrice;
            CreatedAtUtc = createdAtUtc;
        }

        public Guid CustomerId { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatusEnum Status { get; }
        public decimal? TotalPrice { get; }
        public DateTime CreatedAtUtc { get; }
    }
}