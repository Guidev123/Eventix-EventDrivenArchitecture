namespace Eventix.Modules.Ticketing.Application.Orders.Dtos
{
    public record PaymentDto(
        Guid TransactionId,
        decimal Amount,
        string Currency
        );
}