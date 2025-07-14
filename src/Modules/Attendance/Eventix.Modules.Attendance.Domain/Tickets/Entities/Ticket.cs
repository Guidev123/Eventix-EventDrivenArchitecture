using Eventix.Modules.Attendance.Domain.Tickets.DomainEvents;
using Eventix.Modules.Attendance.Domain.Tickets.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Attendance.Domain.Tickets.Entities
{
    public sealed class Ticket : Entity, IAggregateRoot
    {
        public const int TICKET_CODE_LEN = 30;
        public const string TICKET_CODE_PREFIX = "tc_";

        private Ticket(Guid id, Guid attendeeId, Guid eventId, string code, DateTime? usedAtUtc)
        {
            Id = id;
            AttendeeId = attendeeId;
            EventId = eventId;
            Code = code;
            UsedAtUtc = usedAtUtc;
            Validate();
        }

        private Ticket()
        { }

        public Guid AttendeeId { get; private set; }

        public Guid EventId { get; private set; }

        public string Code { get; private set; } = null!;

        public DateTime? UsedAtUtc { get; private set; }

        public static Ticket Create(Guid id, Guid attendeeId, Guid eventId, string code, DateTime? usedAtUtc = null)
        {
            var ticket = new Ticket(id, attendeeId, eventId, code, usedAtUtc);

            ticket.Raise(new TicketCreatedDomainEvent(ticket.Id, ticket.EventId));

            return ticket;
        }

        public void MarkAsUsed()
        {
            UsedAtUtc = DateTime.UtcNow;

            Raise(new TicketUsedDomainEvent(Id));
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(
                EventId.ToString(),
                TicketErrors.InvalidEventId.Description);

            AssertionConcern.EnsureNotEmpty(
                AttendeeId.ToString(),
                TicketErrors.InvalidAttendeeId.Description);

            AssertionConcern.EnsureNotNull(
               Code,
               TicketErrors.CodeIsRequired.Description);

            AssertionConcern.EnsureNotEmpty(
               Code,
               TicketErrors.CodeCannotBeEmpty.Description);

            AssertionConcern.EnsureLengthInRange(
                Code,
                TICKET_CODE_LEN,
                TICKET_CODE_LEN,
                TicketErrors.InvalidCodeLength.Description);

            AssertionConcern.EnsureTrue(
                Code.StartsWith(TICKET_CODE_PREFIX),
                TicketErrors.InvalidCodePrefix.Description);
        }
    }
}