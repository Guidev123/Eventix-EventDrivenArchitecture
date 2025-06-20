using Eventix.Modules.Ticketing.Domain.Orders.Errors;
using Eventix.Modules.Ticketing.Domain.Orders.ValueObjects;
using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Ticketing.Domain.Orders.Entities
{
    public sealed class OrderItem : Entity
    {
        private OrderItem(Guid orderId, Guid ticketTypeId, decimal quantity, decimal unitPrice, string currency)
        {
            OrderId = orderId;
            TicketTypeId = ticketTypeId;
            Quantity = quantity;
            UnitPrice = (unitPrice, currency);
            Price = (quantity * unitPrice, currency);
            Validate();
        }

        private OrderItem()
        { }

        public Guid OrderId { get; private set; }

        public Guid TicketTypeId { get; private set; }

        public Quantity Quantity { get; private set; } = null!;

        public Money UnitPrice { get; private set; } = null!;

        public Money Price { get; private set; } = null!;

        internal static OrderItem Create(Guid orderId, Guid ticketTypeId, decimal quantity, decimal unitPrice, string currency)
            => new(orderId, ticketTypeId, quantity, unitPrice, currency);

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(
                OrderId.ToString(),
                OrderErrors.InvalidOrderId.Description);

            AssertionConcern.EnsureNotEmpty(
                TicketTypeId.ToString(),
                OrderErrors.InvalidTicketTypeId.Description);

            AssertionConcern.EnsureNotNull(
                Quantity,
                OrderErrors.QuantityIsRequired.Description);

            AssertionConcern.EnsureNotNull(
                UnitPrice,
                OrderErrors.UnitPriceIsRequired.Description);

            AssertionConcern.EnsureNotNull(
                Price,
                OrderErrors.PriceIsRequired.Description);
        }
    }
}