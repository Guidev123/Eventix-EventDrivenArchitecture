using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.AddItem
{
    public record AddItemToCartCommand : ICommand
    {
        public AddItemToCartCommand(Guid ticketTypeId, decimal quantity)
        {
            TicketTypeId = ticketTypeId;
            Quantity = quantity;
        }

        public Guid CustomerId { get; private set; }
        public Guid TicketTypeId { get; }
        public decimal Quantity { get; }

        public void SetCustomerId(Guid customerId) => CustomerId = customerId;
    }
}