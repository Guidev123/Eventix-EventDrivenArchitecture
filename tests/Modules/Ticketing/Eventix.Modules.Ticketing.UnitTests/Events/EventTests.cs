using Eventix.Modules.Ticketing.Domain.Events.DomainEvents;
using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.UnitTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Ticketing.UnitTests.Events;

public class EventTests : BaseTest
{
    [Fact(DisplayName = "Create Should Return Value When Event Is Created")]
    [Trait("Ticketing Unit Tests", "Event Tests")]
    public void Create_ShouldReturnValue_WhenEventIsCreated()
    {
        // Act
        var startsAtUtc = DateTime.UtcNow.AddDays(1);
        var result = Event.Create(
            Guid.NewGuid(),
            _faker.Music.Genre(),
            _faker.Lorem.Paragraph(),
            startsAtUtc,
            null);

        // Assert
        result.Value.Should().NotBeNull();
    }

    [Fact(DisplayName = "Reschedule Should Raise Domain Event When Event Is Rescheduled")]
    [Trait("Ticketing Unit Tests", "Event Tests")]
    public void Reschedule_ShouldRaiseDomainEvent_WhenEventIsRescheduled()
    {
        // Arrange
        var startsAtUtc = DateTime.UtcNow.AddDays(1);
        var result = Event.Create(
            Guid.NewGuid(),
            _faker.Music.Genre(),
            _faker.Lorem.Paragraph(),
            startsAtUtc,
            null);

        // Act
        result.Value.Reschedule(
            startsAtUtc.AddDays(1),
            startsAtUtc.AddDays(2));

        // Assert
        var domainEvent = AssertDomainEventWasPublished<EventRescheduledDomainEvent>(result.Value);
        domainEvent.EventId.Should().Be(result.Value.Id);
    }

    [Fact(DisplayName = "Cancel Should Raise Domain Event When Event Is Canceled")]
    [Trait("Ticketing Unit Tests", "Event Tests")]
    public void Cancel_ShouldRaiseDomainEvent_WhenEventIsCanceled()
    {
        // Arrange
        var startsAtUtc = DateTime.UtcNow.AddDays(1);
        var result = Event.Create(
            Guid.NewGuid(),
            _faker.Music.Genre(),
            _faker.Lorem.Paragraph(),
            startsAtUtc,
            null);

        // Act
        result.Value.Cancel();

        // Assert
        var domainEvent = AssertDomainEventWasPublished<EventCancelledDomainEvent>(result.Value);
        domainEvent.EventId.Should().Be(result.Value.Id);
    }

    [Fact(DisplayName = "PaymentsRefunded Should Raise Domain Event When Payments Are Refunded")]
    [Trait("Ticketing Unit Tests", "Event Tests")]
    public void PaymentsRefunded_ShouldRaiseDomainEvent_WhenPaymentsAreRefunded()
    {
        // Arrange
        var startsAtUtc = DateTime.UtcNow.AddDays(1);
        var result = Event.Create(
            Guid.NewGuid(),
            _faker.Music.Genre(),
            _faker.Lorem.Paragraph(),
            startsAtUtc,
            null);

        // Act
        result.Value.PaymentsRefunded();

        // Assert
        var domainEvent = AssertDomainEventWasPublished<EventPaymentsRefundedDomainEvent>(result.Value);
        domainEvent.EventId.Should().Be(result.Value.Id);
    }

    [Fact(DisplayName = "TicketsArchived Should Raise Domain Event When Tickets Are Archived")]
    [Trait("Ticketing Unit Tests", "Event Tests")]
    public void TicketsArchived_ShouldRaiseDomainEvent_WhenTicketsAreArchived()
    {
        // Arrange
        var startsAtUtc = DateTime.UtcNow.AddDays(1);
        var result = Event.Create(
            Guid.NewGuid(),
            _faker.Music.Genre(),
            _faker.Lorem.Paragraph(),
            startsAtUtc,
            null);

        // Act
        result.Value.TicketsArchived();

        // Assert
        var domainEvent = AssertDomainEventWasPublished<EventTicketsArchivedDomainEvent>(result.Value);
        domainEvent.EventId.Should().Be(result.Value.Id);
    }
}