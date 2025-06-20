namespace Eventix.Modules.Ticketing.Application.Tickets.Dtos
{
    public record TicketDto(
        Guid Id,
        Guid CustomerId,
        Guid OrderId,
        Guid EventId,
        Guid TicketTypeId,
        string Code,
        DateTime CreatedAtUtc
        );
}