using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.DomainEvents;
using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.Errors;
using Eventix.Modules.Ticketing.UnitTests.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;

namespace Eventix.Modules.Ticketing.UnitTests.Orders;

public class OrderTests : BaseTest
{
    [Fact(DisplayName = "Create Should Raise Domain Event When Order Is Created")]
    [Trait("Ticketing Unit Tests", "Order Tests")]
    public void Create_ShouldRaiseDomainEvent_WhenOrderIsCreated()
    {
        // Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Name.FirstName(),
            _faker.Name.LastName());

        // Act
        Result<Order> result = Order.Create(customer);

        // Assert
        var domainEvent =
            AssertDomainEventWasPublished<OrderCreatedDomainEvent>(result.Value);

        domainEvent.OrderId.Should().Be(result.Value.Id);
    }

    [Fact(DisplayName = "IssueTicket Should Return Failure When Ticket Already Issued")]
    [Trait("Ticketing Unit Tests", "Order Tests")]
    public void IssueTicket_ShouldReturnFailure_WhenTicketAlreadyIssued()
    {
        // Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Name.FirstName(),
            _faker.Name.LastName());

        Result<Order> result = Order.Create(customer);

        result.Value.IssueTickets();
        Order order = result.Value;

        // Act
        Result issueTicketsResult = order.IssueTickets();

        // Assert
        issueTicketsResult.Error.Should().Be(OrderErrors.TicketsAlreadyIssues);
    }

    [Fact(DisplayName = "IssueTicket Should Raise Domain Event When Ticket Is Issued")]
    [Trait("Ticketing Unit Tests", "Order Tests")]
    public void IssueTicket_ShouldRaiseDomainEvent_WhenTicketIsIssued()
    {
        // Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Name.FirstName(),
            _faker.Name.LastName());

        Result<Order> result = Order.Create(customer);

        // Act
        result.Value.IssueTickets();

        // Assert
        var domainEvent =
            AssertDomainEventWasPublished<OrderTicketsIssuedDomainEvent>(result.Value);

        domainEvent.OrderId.Should().Be(result.Value.Id);
    }
}