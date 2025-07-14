using Eventix.Modules.Attendance.Domain.Attendees.DomainEvents;
using Eventix.Modules.Attendance.Domain.Attendees.Errors;
using Eventix.Modules.Attendance.Domain.Tickets.Entities;
using Eventix.Modules.Attendance.Domain.Tickets.Errors;
using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.Responses;
using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Attendance.Domain.Attendees.Entities
{
    public sealed class Attendee : Entity, IAggregateRoot
    {
        private Attendee(Guid id, string email, string firstName, string lastName)
        {
            Id = id;
            Email = email;
            Name = (firstName, lastName);
            Validate();
        }

        private Attendee()
        { }

        public Email Email { get; private set; } = null!;
        public Name Name { get; private set; } = null!;

        public static Attendee Create(Guid id, string email, string firstName, string lastName)
        {
            var attendee = new Attendee(id, email, firstName, lastName);

            return attendee;
        }

        public void UpdateName(string firstName, string lastName)
        {
            if (Name.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase)
                && Name.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)) return;

            var name = (firstName, lastName);
            Name = name;
        }

        public void UpdateEmail(string email)
        {
            if (Email.Address.Equals(email,
                StringComparison.OrdinalIgnoreCase)) return;

            Email = email;
        }

        public Result CheckIn(Ticket ticket)
        {
            if (Id != ticket.AttendeeId)
            {
                Raise(new InvalidCheckInAttemptDomainEvent(Id, ticket.EventId, ticket.Id, ticket.Code));

                return Result.Failure(TicketErrors.InvalidCheckIn);
            }

            if (ticket.UsedAtUtc.HasValue)
            {
                Raise(new DuplicateCheckInAttemptDomainEvent(Id, ticket.EventId, ticket.Id, ticket.Code));

                return Result.Failure(TicketErrors.DuplicateCheckIn);
            }

            ticket.MarkAsUsed();

            Raise(new AttendeeCheckedInDomainEvent(Id, ticket.EventId));

            return Result.Success();
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotNull(Email, AttendeeErrors.EmailMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotNull(Name, AttendeeErrors.NameMustBeNotEmpty.Description);
        }
    }
}