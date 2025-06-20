using Eventix.Modules.Ticketing.Domain.Orders.Enumerators;
using System.Text.Json.Serialization;

namespace Eventix.Modules.Ticketing.Application.Orders.UseCases.GetByCustomer
{
    public record GetOrdersByCustomerResponse
    {
        public GetOrdersByCustomerResponse(Guid id, Guid customerId, OrderStatusEnum status, DateTime createdAtUtc, decimal? totalPrice = null)
        {
            Id = id;
            CustomerId = customerId;
            Status = status;
            TotalPrice = totalPrice;
            CreatedAtUtc = createdAtUtc;
        }
        public Guid Id { get; private set; }
        public Guid CustomerId { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatusEnum Status { get; }
        public decimal? TotalPrice { get; }
        public DateTime CreatedAtUtc { get; }
    }
}