namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetById
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