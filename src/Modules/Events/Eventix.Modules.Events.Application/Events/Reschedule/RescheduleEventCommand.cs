using Eventix.Modules.Events.Application.Abstractions.Messaging;

namespace Eventix.Modules.Events.Application.Events.Reschedule
{
    public record RescheduleEventCommand(
        Guid EventId,
        DateTime StartsAtUtc,
        DateTime? EndsAtUtc) : ICommand
    {
    }
}