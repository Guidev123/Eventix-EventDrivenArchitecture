using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Payments.UseCases.PayOrder
{
    public sealed record PayOrderCommand(
        Guid OrderId,
        decimal TotalPrice,
        string Currency
        ) : ICommand;
}