namespace Eventix.Modules.Attendance.Domain.Events.Models
{
    public sealed class DuplicateCheckInTicket : CheckInTicket
    {
        public DuplicateCheckInTicket(
            string code,
            Guid eventId,
            int count
            ) : base(code, eventId, count) { }

        private DuplicateCheckInTicket()
            : base(default!, default!, default!) { }
    }
}