using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Payments.UseCases.RefundForEvent
{
    public record RefundPaymentsForEventCommand(Guid EventId) : ICommand;
}