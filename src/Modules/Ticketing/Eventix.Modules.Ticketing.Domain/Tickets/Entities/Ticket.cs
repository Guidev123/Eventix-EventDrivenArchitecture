using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Modules.Ticketing.Domain.Tickets.DomainEvents;
using Eventix.Modules.Ticketing.Domain.Tickets.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Ticketing.Domain.Tickets.Entities
{
    public sealed class Ticket : Entity, IAggregateRoot
    {
        public const int TICKET_CODE_LEN = 30;
        public const string TICKET_CODE_PREFIX = "tc_";

        private Ticket(Guid customerId, Guid orderId, Guid eventId, Guid ticketTypeId)
        {
            CustomerId = customerId;
            OrderId = orderId;
            EventId = eventId;
            TicketTypeId = ticketTypeId;
            Code = $"tc_{Guid.NewGuid().ToString("N")[..27]}";
            CreatedAtUtc = DateTime.UtcNow;
            IsArchived = false;
            Validate();
        }

        private Ticket()
        { }

        public Guid CustomerId { get; private set; }

        public Guid OrderId { get; private set; }

        public Guid EventId { get; private set; }

        public Guid TicketTypeId { get; private set; }

        public string Code { get; private set; } = string.Empty;

        public DateTime CreatedAtUtc { get; private set; }

        public bool IsArchived { get; private set; }

        public static Ticket Create(Order order, TicketType ticketType)
        {
            var ticket = new Ticket(order.CustomerId, order.Id, ticketType.EventId, ticketType.Id);

            ticket.Raise(new TicketCreatedDomainEvent(ticket.Id));

            return ticket;
        }

        public void Archive()
        {
            if (!IsArchived)
            {
                IsArchived = true;

                Raise(new TicketArchivedDomainEvent(Id, Code));
            }
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(
                CustomerId.ToString(),
                TicketErrors.InvalidCustomerId.Description);

            AssertionConcern.EnsureNotEmpty(
                OrderId.ToString(),
                TicketErrors.InvalidOrderId.Description);

            AssertionConcern.EnsureNotEmpty(
                EventId.ToString(),
                TicketErrors.InvalidEventId.Description);

            AssertionConcern.EnsureNotEmpty(
                TicketTypeId.ToString(),
                TicketErrors.InvalidTicketTypeId.Description);

            AssertionConcern.EnsureNotNull(
                Code,
                TicketErrors.CodeIsRequired.Description);

            AssertionConcern.EnsureFalse(
                CreatedAtUtc == default,
                TicketErrors.InvalidCreatedDate.Description);

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