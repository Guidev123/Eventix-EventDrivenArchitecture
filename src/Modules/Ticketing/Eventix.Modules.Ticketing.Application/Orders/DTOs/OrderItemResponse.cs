namespace Eventix.Modules.Ticketing.Application.Orders.Dtos
{
    public sealed record OrderItemResponse(
        Guid OrderItemId,
        Guid OrderId,
        Guid TicketTypeId,
        decimal Quantity,
        decimal UnitPrice,
        decimal Price,
        string Currency
        );
}