using Eventix.Modules.Events.Domain.Categories.Entities;
using Eventix.Modules.Events.Domain.Events.DomainEvents;
using Eventix.Modules.Events.Domain.Events.Entities;
using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.UnitTests.Abstractions;
using FluentAssertions;
using Xunit;

namespace Eventix.Modules.Events.UnitTests.Events
{
    public class EventTest : BaseTest
    {
        [Fact(DisplayName = "Create Should Return Failure When End Date Precedes Start Date")]
        [Trait("Events Unit Tests", "Domain Tests")]
        public void Create_ShouldReturnFailure_WhenEndDatePrecedesStartDate()
        {
            // Arrange
            var category = Category.Create(_faker.Music.Genre());
            var startsAtUtc = DateTime.Now.AddDays(1);
            var endsAtUtc = DateTime.Now.AddMinutes(-1);

            // Act
            var result = Event.Create(
                _faker.Music.Genre(),
                _faker.Lorem.Paragraph(),
                category.Id,
                startsAtUtc,
                endsAtUtc
                );

            // Assert
            result.Error.Should().Be(EventErrors.EndDatePrecedesStartDate);
        }

        [Fact(DisplayName = "Create Should Raise Domain Event When Event Created")]
        [Trait("Events Unit Tests", "Domain Tests")]
        public void Create_ShouldRaiseDomainEvent_WhenEventCreated()
        {
            // Arrange
            var category = Category.Create(_faker.Music.Genre());
            var startsAtUtc = DateTime.Now.AddDays(1);

            // Act
            var result = Event.Create(
                _faker.Music.Genre(),
                _faker.Lorem.Paragraph(),
                category.Id,
                startsAtUtc,
                null
                );

            var @event = result.Value;

            // Assert
            var domainEvent = AssertDomainEventWasPublished<EventCreatedDomainEvent>(@event);
            domainEvent.EventId.Should().Be(@event.Id);
        }
    }
}