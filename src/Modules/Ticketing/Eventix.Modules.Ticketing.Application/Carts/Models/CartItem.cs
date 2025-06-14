namespace Eventix.Modules.Ticketing.Application.Carts.Models
{
    public sealed class CartItem
    {
        public Guid TicketTypeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;

        public void UpdateQuantity(decimal quantity)
        {
            Quantity += quantity;
        }
    }
}