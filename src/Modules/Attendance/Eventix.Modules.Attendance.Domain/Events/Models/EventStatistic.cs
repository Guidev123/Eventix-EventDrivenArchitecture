using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Attendance.Domain.Events.Models
{
    public sealed class EventStatistic
    {
        private EventStatistic(
            Guid eventId,
            string title,
            string description,
            Location? location,
            DateTime startsAtUtc,
            DateTime? endsAtUtc
            )
        {
            EventId = eventId;
            Title = title;
            Description = description;
            Location = location;
            StartsAtUtc = startsAtUtc;
            EndsAtUtc = endsAtUtc;
            TicketsSold = (int)decimal.Zero;
            AttendeesCheckedIn = (int)decimal.Zero;
            DuplicateCheckInTickets = [];
            InvalidCheckInTickets = [];
        }

        private EventStatistic()
        { }

        public Guid EventId { get; private set; }

        public string Title { get; private set; } = default!;

        public string Description { get; private set; } = default!;

        public Location? Location { get; private set; }

        public DateTime StartsAtUtc { get; private set; }

        public DateTime? EndsAtUtc { get; private set; }

        public int TicketsSold { get; private set; }

        public int AttendeesCheckedIn { get; private set; }

        public List<DuplicateCheckInTicket> DuplicateCheckInTickets { get; private set; } = [];

        public List<InvalidCheckInTicket> InvalidCheckInTickets { get; private set; } = [];

        public static EventStatistic Create(
            Guid eventId,
            string title,
            string description,
            Location? location,
            DateTime startsAtUtc,
            DateTime? endsAtUtc
            )
        {
            var eventStatistics = new EventStatistic(
                eventId,
                title,
                description,
                location,
                startsAtUtc,
                endsAtUtc
                );

            return eventStatistics;
        }
    }
}