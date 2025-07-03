using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.DomainEvents;
using Eventix.Modules.Ticketing.Domain.Orders.Enumerators;
using Eventix.Modules.Ticketing.Domain.Orders.Errors;
using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.Responses;
using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Ticketing.Domain.Orders.Entities
{
    public sealed class Order : Entity, IAggregateRoot
    {
        private Order(Guid customerId)
        {
            CustomerId = customerId;
            Status = OrderStatusEnum.Pending;
            TicketsIssued = false;
            CreatedAtUtc = DateTime.UtcNow;
            Validate();
        }

        private Order()
        { }

        public Guid CustomerId { get; private set; }

        public OrderStatusEnum Status { get; private set; }

        public Money? TotalPrice { get; private set; }

        public bool TicketsIssued { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
        private readonly List<OrderItem> _orderItems = [];

        public static Order Create(Customer customer)
        {
            var order = new Order(customer.Id);

            order.Raise(new OrderCreatedDomainEvent(order.Id));

            return order;
        }

        public void AddItem(TicketType ticketType, decimal quantity, decimal price, string currency)
        {
            var orderItem = OrderItem.Create(Id, ticketType.Id, quantity, price, currency);

            _orderItems.Add(orderItem);

            var totalPrice = _orderItems.Sum(o => o.Price.Amount);
            TotalPrice = (totalPrice, currency);
        }

        public Result IssueTickets()
        {
            if (TicketsIssued)
            {
                return Result.Failure(OrderErrors.TicketsAlreadyIssues);
            }

            TicketsIssued = true;

            Raise(new OrderTicketsIssuedDomainEvent(Id));

            return Result.Success();
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(
                CustomerId.ToString(),
                OrderErrors.InvalidCustomerId.Description);

            AssertionConcern.EnsureTrue(
                Enum.IsDefined(Status),
                OrderErrors.InvalidStatus.Description);

            AssertionConcern.EnsureFalse(
                CreatedAtUtc == default,
                OrderErrors.InvalidCreationDate.Description);
        }
    }
}