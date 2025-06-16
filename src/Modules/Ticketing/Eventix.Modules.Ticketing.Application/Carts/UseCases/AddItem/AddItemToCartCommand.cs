using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.AddItem
{
    public record AddItemToCartCommand(
        Guid CustomerId,
        Guid TicketTypeId,
        decimal Quantity
        ) : ICommand;
}