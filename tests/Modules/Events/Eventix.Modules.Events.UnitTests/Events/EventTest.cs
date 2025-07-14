using Eventix.Modules.Events.Domain.Categories.Entities;
using Eventix.Modules.Events.Domain.Events.DomainEvents;
using Eventix.Modules.Events.Domain.Events.Entities;
using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.UnitTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Events.UnitTests.Events
{
    public class EventTest : BaseTest
    {
        [Fact(DisplayName = "Publish Should Return Failure When Event Not Draft")]
        [Trait("Events Unit Tests", "Event Tests")]
        public void Publish_ShouldReturnFailure_WhenEventNotDraft()
        {
            // Arrange
            var category = Category.Create(_faker.Music.Genre());
            var startsAtUtc = DateTime.UtcNow.AddDays(1);

            var result = Event.Create(
                _faker.Music.Genre(),
                _faker.Lorem.Paragraph(),
                category.Id,
                startsAtUtc,
                null);

            var @event = result.Value;

            @event.Publish();

            // Act
            var publishResult = @event.Publish();

            // Assert
            publishResult.Error.Should().Be(EventErrors.NotDraft);
        }

        [Fact(DisplayName = "Publish Should Raise Domain Event When Event Published")]
        [Trait("Events Unit Tests", "Event Tests")]
        public void Publish_ShouldRaiseDomainEvent_WhenEventPublished()
        {
            // Arrange
            var category = Category.Create(_faker.Music.Genre());
            var startsAtUtc = DateTime.UtcNow.AddDays(1);

            var result = Event.Create(
                _faker.Music.Genre(),
                _faker.Lorem.Paragraph(),
                category.Id,
                startsAtUtc,
                null);

            var @event = result.Value;

            // Act
            @event.Publish();

            // Assert
            var domainEvent = AssertDomainEventWasPublished<EventPublishedDomainEvent>(@event);

            domainEvent.EventId.Should().Be(@event.Id);
        }

        [Fact(DisplayName = "Reschedule Should Raise Domain Event When Event Rescheduled")]
        [Trait("Events Unit Tests", "Event Tests")]
        public void Reschedule_ShouldRaiseDomainEvent_WhenEventRescheduled()
        {
            // Arrange
            var category = Category.Create(_faker.Music.Genre());
            var startsAtUtc = DateTime.UtcNow.AddDays(1);

            var result = Event.Create(
                _faker.Music.Genre(),
                _faker.Lorem.Paragraph(),
                category.Id,
                startsAtUtc,
                null);

            var @event = result.Value;

            // Act
            @event.Reschedule(startsAtUtc.AddDays(1), startsAtUtc.AddDays(2));

            // Assert
            var domainEvent = AssertDomainEventWasPublished<EventRescheduledDomainEvent>(@event);

            domainEvent.EventId.Should().Be(@event.Id);
        }

        [Fact(DisplayName = "Cancel Should Raise Domain Event When Event Canceled")]
        [Trait("Events Unit Tests", "Event Tests")]
        public void Cancel_ShouldRaiseDomainEvent_WhenEventCanceled()
        {
            // Arrange
            var category = Category.Create(_faker.Music.Genre());
            var startsAtUtc = DateTime.UtcNow.AddDays(1);

            var result = Event.Create(
                _faker.Music.Genre(),
                _faker.Lorem.Paragraph(),
                category.Id,
                startsAtUtc,
                null);

            var @event = result.Value;

            // Act
            @event.Cancel(startsAtUtc.AddMinutes(-1));

            // Assert
            var domainEvent = AssertDomainEventWasPublished<EventCancelledDomainEvent>(@event);

            domainEvent.EventId.Should().Be(@event.Id);
        }

        [Fact(DisplayName = "Cancel Should Return Failure When Event Already Canceled")]
        [Trait("Events Unit Tests", "Event Tests")]
        public void Cancel_ShouldReturnFailure_WhenEventAlreadyCanceled()
        {
            // Arrange
            var category = Category.Create(_faker.Music.Genre());
            var startsAtUtc = DateTime.UtcNow.AddDays(1);

            var result = Event.Create(
                _faker.Music.Genre(),
                _faker.Lorem.Paragraph(),
                category.Id,
                startsAtUtc,
                null);

            var @event = result.Value;

            @event.Cancel(startsAtUtc.AddMinutes(-1));

            // Act
            var cancelResult = @event.Cancel(startsAtUtc.AddMinutes(-1));

            // Assert
            cancelResult.Error.Should().Be(EventErrors.AlreadyCanceled);
        }

        [Fact(DisplayName = "Cancel Should Return Failure When Event Already Started")]
        [Trait("Events Unit Tests", "Event Tests")]
        public void Cancel_ShouldReturnFailure_WhenEventAlreadyStarted()
        {
            // Arrange
            var category = Category.Create(_faker.Music.Genre());
            var startsAtUtc = DateTime.UtcNow.AddDays(1);

            var result = Event.Create(
                _faker.Music.Genre(),
                _faker.Lorem.Paragraph(),
                category.Id,
                startsAtUtc,
                null);

            var @event = result.Value;

            // Act
            var cancelResult = @event.Cancel(startsAtUtc.AddMinutes(1));

            // Assert
            cancelResult.Error.Should().Be(EventErrors.AlreadyStarted);
        }
    }
}