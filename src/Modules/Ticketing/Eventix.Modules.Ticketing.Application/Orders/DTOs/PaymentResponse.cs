namespace Eventix.Modules.Ticketing.Application.Orders.Dtos
{
    public record PaymentResponse(
        Guid TransactionId,
        decimal Amount,
        string Currency
        );
}