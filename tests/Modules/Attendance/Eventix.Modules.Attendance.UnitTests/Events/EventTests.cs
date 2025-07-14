using Eventix.Modules.Attendance.Domain.Events.DomainEvents;
using Eventix.Modules.Attendance.Domain.Events.Entities;
using Eventix.Modules.Attendance.UnitTests.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;

namespace Eventix.Modules.Attendance.UnitTests.Events;

public class EventTests : BaseTest
{
    [Fact(DisplayName = "Should Raise Domain Event When Event Created")]
    [Trait("Attendance Unit Tests", "Event Tests")]
    public void Should_RaiseDomainEvent_WhenEventCreated()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var startsAtUtc = DateTime.UtcNow.AddHours(1);

        // Act
        Result<Event> @event = Event.Create(
            Guid.NewGuid(),
            _faker.Music.Genre(),
            _faker.Lorem.Paragraph(),
            startsAtUtc);

        // Assert
        var domainEvent = AssertDomainEventWasPublished<EventCreatedDomainEvent>(@event.Value);

        domainEvent.EventId.Should().Be(@event.Value.Id);
    }
}