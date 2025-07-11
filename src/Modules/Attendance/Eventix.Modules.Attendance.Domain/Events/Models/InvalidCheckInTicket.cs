namespace Eventix.Modules.Attendance.Domain.Events.Models
{
    public sealed class InvalidCheckInTicket : CheckInTicket
    {
        public InvalidCheckInTicket(
            string code,
            Guid eventId,
            int count
            ) : base(code, eventId, count) { }

        private InvalidCheckInTicket()
            : base(default!, default!, default!) { }
    }
}