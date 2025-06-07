namespace Eventix.Modules.Events.Application.TicketTypes.GetById
{
    public record GetTicketTypeByIdResponse(
        Guid EventId,
        string Name,
        decimal Price,
        string Currency,
        decimal Quantity
        );
}