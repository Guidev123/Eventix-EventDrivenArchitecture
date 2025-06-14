namespace Eventix.Modules.Events.PublicApi.TicketTypes.Responses
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