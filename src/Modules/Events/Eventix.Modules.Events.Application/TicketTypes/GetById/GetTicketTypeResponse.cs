namespace Eventix.Modules.Events.Application.TicketTypes.GetById
{
    public record GetTicketTypeResponse(
        Guid Id,
        Guid EventId,
        string Name,
        decimal Amount,
        string Currency,
        decimal Quantity
        );
}