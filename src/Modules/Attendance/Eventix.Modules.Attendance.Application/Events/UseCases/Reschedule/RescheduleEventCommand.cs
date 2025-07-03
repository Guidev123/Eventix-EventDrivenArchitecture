using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Attendance.Application.Events.UseCases.Reschedule
{
    public sealed record RescheduleEventCommand(
        Guid EventId,
        DateTime StartsAtUtc,
        DateTime? EndsAtUtc) : ICommand;
}