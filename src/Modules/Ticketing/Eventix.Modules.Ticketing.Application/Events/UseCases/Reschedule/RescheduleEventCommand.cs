using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Events.UseCases.Reschedule
{
    public record RescheduleEventCommand(
        Guid EventId,
        DateTime StartsAtUtc,
        DateTime? EndsAtUtc) : ICommand;
}