namespace Eventix.Modules.Events.Application.TicketTypes.GetAll
{
    public record GetAllTicketTypesResponse(
        Guid Id,
        Guid EventId,
        string Name,
        decimal Price,
        string Currency,
        decimal Quantity
        );
}