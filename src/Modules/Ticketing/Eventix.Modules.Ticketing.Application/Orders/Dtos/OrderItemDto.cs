namespace Eventix.Modules.Ticketing.Application.Orders.Dtos
{
    public record OrderItemDto(
        Guid OrderItemId,
        Guid OrderId,
        Guid TicketTypeId,
        decimal Quantity,
        decimal UnitPrice,
        decimal Price,
        string Currency
        );
}