using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Modules.Ticketing.Domain.Tickets.DomainEvents;
using Eventix.Modules.Ticketing.Domain.Tickets.Entities;
using Eventix.Modules.Ticketing.UnitTests.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;

namespace Eventix.Modules.Ticketing.UnitTests.Tickets;

public class TicketTests : BaseTest
{
    [Fact(DisplayName = "Create Should Raise Domain Event When Ticket Is Created")]
    [Trait("Ticketing Unit Tests", "Ticket Tests")]
    public void Create_ShouldRaiseDomainEvent_WhenTicketIsCreated()
    {
        // Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Name.FirstName(),
            _faker.Name.LastName());

        var order = Order.Create(customer);

        var startsAtUtc = DateTime.UtcNow.AddDays(1);
        var @event = Event.Create(
            Guid.NewGuid(),
            _faker.Music.Genre(),
            _faker.Lorem.Paragraph(),
            startsAtUtc,
            null);

        decimal quantity = _faker.Random.Decimal(1, 1000000000);
        Result<TicketType> ticketType = TicketType.Create(
            Guid.NewGuid(),
            @event.Value.Id,
            _faker.Name.FirstName(),
            _faker.Random.Decimal(1, 1000000000),
            "USD",
            quantity);

        // Act
        Result<Ticket> result = Ticket.Create(
            order,
            ticketType.Value);

        // Assert
        var domainEvent =
            AssertDomainEventWasPublished<TicketCreatedDomainEvent>(result.Value);

        domainEvent.TicketId.Should().Be(result.Value.Id);
    }

    [Fact(DisplayName = "Archive Should Raise Domain Event When Ticket Is Archived")]
    [Trait("Ticketing Unit Tests", "Ticket Tests")]
    public void Archive_ShouldRaiseDomainEvent_WhenTicketIsArchived()
    {
        // Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Name.FirstName(),
            _faker.Name.LastName());

        var order = Order.Create(customer);

        var startsAtUtc = DateTime.UtcNow.AddDays(1);
        var @event = Event.Create(
            Guid.NewGuid(),
            _faker.Music.Genre(),
            _faker.Lorem.Paragraph(),
            startsAtUtc,
            null);

        decimal quantity = _faker.Random.Decimal(1, 1000000000);
        Result<TicketType> ticketType = TicketType.Create(
            Guid.NewGuid(),
            @event.Value.Id,
            _faker.Name.FirstName(),
            _faker.Random.Decimal(1, 1000000000),
            "USD",
            quantity);

        Result<Ticket> result = Ticket.Create(
            order,
            ticketType.Value);

        // Act
        result.Value.Archive();

        // Assert
        TicketArchivedDomainEvent domainEvent =
            AssertDomainEventWasPublished<TicketArchivedDomainEvent>(result.Value);

        domainEvent.TicketId.Should().Be(result.Value.Id);
    }
}