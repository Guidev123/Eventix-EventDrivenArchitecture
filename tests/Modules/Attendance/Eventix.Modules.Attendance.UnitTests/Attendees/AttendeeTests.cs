using Eventix.Modules.Attendance.Domain.Attendees.DomainEvents;
using Eventix.Modules.Attendance.Domain.Attendees.Entities;
using Eventix.Modules.Attendance.Domain.Events.Entities;
using Eventix.Modules.Attendance.Domain.Tickets.Entities;
using Eventix.Modules.Attendance.Domain.Tickets.Errors;
using Eventix.Modules.Attendance.UnitTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Attendance.UnitTests.Attendees;

public class AttendeeTests : BaseTest
{
    [Fact(DisplayName = "CheckIn Should Return Failure When Ticket Is Not Valid")]
    [Trait("Attendance Unit Tests", "Attendee Tests")]
    public void CheckIn_ShouldReturnFailure_WhenTicketIsNotValid()
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
            $"tc_{_faker.Random.String(27)}",
            DateTime.UtcNow);

        // Act
        var checkInAttendee = attendee.CheckIn(ticket);

        // Assert
        var domainEvent = AssertDomainEventWasPublished<DuplicateCheckInAttemptDomainEvent>(attendee);

        domainEvent.AttendeeId.Should().Be(attendee.Id);
        checkInAttendee.Error.Should().Be(TicketErrors.DuplicateCheckIn);
    }

    [Fact(DisplayName = "CheckIn Should Return Failure When Ticket Already Used")]
    [Trait("Attendance Unit Tests", "Attendee Tests")]
    public void CheckIn_ShouldReturnFailure_WhenTicketAlreadyUsed()
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
            $"tc_{_faker.Random.String(27)}");

        ticket.MarkAsUsed();

        // Act
        var checkInAttendee = attendee.CheckIn(ticket);

        // Assert
        var domainEvent = AssertDomainEventWasPublished<DuplicateCheckInAttemptDomainEvent>(attendee);

        domainEvent.AttendeeId.Should().Be(attendee.Id);
        checkInAttendee.Error.Should().Be(TicketErrors.DuplicateCheckIn);
    }

    [Fact(DisplayName = "CheckIn Should Raise Domain Event When Successfully Checked In")]
    [Trait("Attendance Unit Tests", "Attendee Tests")]
    public void CheckIn_ShouldRaiseDomainEvent_WhenSuccessfullyCheckedIn()
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
            $"tc_{_faker.Random.String(27)}");

        // Act
        attendee.CheckIn(ticket);

        // Assert
        var domainEvent = AssertDomainEventWasPublished<AttendeeCheckedInDomainEvent>(attendee);

        domainEvent.AttendeeId.Should().Be(attendee.Id);
    }
}