using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Payments.UseCases.RefundForEvent
{
    public sealed record RefundPaymentsForEventCommand(Guid EventId) : ICommand;
}