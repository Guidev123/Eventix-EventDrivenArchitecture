using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.AddItemToCart
{
    public record AddItemToCartCommand(
        Guid CustomerId,
        Guid TicketTypeId,
        decimal Quantity
        ) : ICommand;
}