namespace Eventix.Modules.Attendance.Domain.Events.Models
{
    public abstract class CheckInTicket
    {
        public CheckInTicket(string code, Guid eventId, int count)
        {
            Code = code;
            EventId = eventId;
            Count = count;
        }

        private CheckInTicket()
        { }

        public Guid EventId { get; init; }
        public string Code { get; init; } = default!;
        public int Count { get; private set; }

        public void IncrementCount() => Count += 1;
    }
}