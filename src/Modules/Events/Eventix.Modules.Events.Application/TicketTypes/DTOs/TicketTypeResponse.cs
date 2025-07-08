namespace Eventix.Modules.Events.Application.TicketTypes.DTOs
{
    public sealed record TicketTypeResponse(
        Guid Id,
        Guid EventId,
        string Name,
        decimal Amount,
        string Currency,
        decimal Quantity
        );
}