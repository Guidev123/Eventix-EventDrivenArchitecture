namespace Eventix.Modules.Ticketing.Application.Carts.Models
{
    public sealed class Cart
    {
        public Guid CustomerId { get; set; }

        public List<CartItem> Items { get; set; } = [];
    }
}