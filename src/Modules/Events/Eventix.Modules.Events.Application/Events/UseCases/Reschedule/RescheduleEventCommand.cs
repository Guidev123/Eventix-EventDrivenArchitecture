using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.UseCases.Reschedule
{
    public record RescheduleEventCommand : ICommand
    {
        public RescheduleEventCommand(DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            StartsAtUtc = startsAtUtc;
            EndsAtUtc = endsAtUtc;
        }

        public Guid? EventId { get; private set; }
        public DateTime StartsAtUtc { get; private set; }
        public DateTime? EndsAtUtc { get; private set; }
        public void SetEventId(Guid id) => EventId = id;
    }
}