using Eventix.Modules.Attendance.Domain.Attendees.Entities;
using Eventix.Modules.Attendance.Domain.Events.Entities;
using Eventix.Modules.Attendance.Domain.Tickets.DomainEvents;
using Eventix.Modules.Attendance.Domain.Tickets.Entities;
using Eventix.Modules.Attendance.UnitTests.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;

namespace Eventix.Modules.Attendance.UnitTests.Tickets;

public class TicketTests : BaseTest
{
    [Fact(DisplayName = "Should Raise Domain Event When Ticket Is Created")]
    [Trait("Ticketing Unit Tests", "Ticket Tests")]
    public void Create_ShouldRaiseDomainEvent_WhenTicketIsCreated()
    {
        // Arrange
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Person.FirstName,
            _faker.Person.LastName);

        var startsAtUtc = DateTime.UtcNow.AddHours(1);

        var @event = Event.Create(
            Guid.NewGuid(),
            _faker.Music.Genre(),
            _faker.Lorem.Paragraph(),
            startsAtUtc);

        // Act
        Result<Ticket> ticket = Ticket.Create(
            Guid.NewGuid(),
            attendee.Id,
            @event.Id,
            "tc_" + _faker.Random.String(27));

        // Assert
        var domainEvent =
            AssertDomainEventWasPublished<TicketCreatedDomainEvent>(ticket.Value);

        domainEvent.TicketId.Should().Be(ticket.Value.Id);
    }

    [Fact(DisplayName = "Should Raise Domain Event When Ticket Is Used")]
    [Trait("Ticketing Unit Tests", "Ticket Tests")]
    public void MarkAsUsed_ShouldRaiseDomainEvent_WhenTicketIsUsed()
    {
        // Arrange
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Person.FirstName,
            _faker.Person.LastName);

        var startsAtUtc = DateTime.UtcNow.AddHours(1);

        var @event = Event.Create(
            Guid.NewGuid(),
            _faker.Music.Genre(),
            _faker.Lorem.Paragraph(),
            startsAtUtc);

        var ticket = Ticket.Create(
            Guid.NewGuid(),
            attendee.Id,
            @event.Id,
            "tc_" + _faker.Random.String(27));

        // Act
        ticket.MarkAsUsed();

        // Assert
        var domainEvent = AssertDomainEventWasPublished<TicketUsedDomainEvent>(ticket);

        domainEvent.TicketId.Should().Be(ticket.Id);
    }
}