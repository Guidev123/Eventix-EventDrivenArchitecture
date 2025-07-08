namespace Eventix.Modules.Ticketing.Application.Tickets.DTOs
{
    public record TicketResponse(
        Guid Id,
        Guid CustomerId,
        Guid OrderId,
        Guid EventId,
        Guid TicketTypeId,
        string Code,
        DateTime CreatedAtUtc
        );
}