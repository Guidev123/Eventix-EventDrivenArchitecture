using Eventix.Modules.Events.Domain.Categories.Entities;
using Eventix.Modules.Events.Domain.Events.Entities;
using Eventix.Modules.Events.Domain.TicketTypes.DomainEvents;
using Eventix.Modules.Events.Domain.TicketTypes.Entities;
using Eventix.Modules.Events.UnitTests.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;

namespace Eventix.Modules.Events.UnitTests.TicketTypes
{
    public class TicketTypeTests : BaseTest
    {
        [Fact(DisplayName = "Create Should Return Value When TicketType Is Created")]
        [Trait("Events Unit Tests", "TicketTypes Tests")]
        public void Create_ShouldReturnValue_WhenTicketTypeIsCreated()
        {
            // Arrange
            var category = Category.Create(_faker.Music.Genre());
            var startsAtUtc = DateTime.UtcNow.AddDays(1);

            var eventResult = Event.Create(
                _faker.Music.Genre(),
                _faker.Lorem.Paragraph(),
                category.Id,
                startsAtUtc,
                null);

            // Act
            Result<TicketType> result = TicketType.Create(
                eventResult.Value.Id,
                _faker.Music.Genre(),
                _faker.Random.Decimal(1, 1000000000),
                "USD",
                _faker.Random.Decimal(1, 1000000000));

            // Assert
            result.Value.Should().NotBeNull();
        }

        [Fact(DisplayName = "Update Price Should Raise Domain Event When TicketType Is Updated")]
        [Trait("Events Unit Tests", "TicketTypes Tests")]
        public void UpdatePrice_ShouldRaiseDomainEvent_WhenTicketTypeIsUpdated()
        {
            // Arrange
            var category = Category.Create(_faker.Music.Genre());
            var startsAtUtc = DateTime.UtcNow.AddDays(1);

            var eventResult = Event.Create(
                _faker.Music.Genre(),
                _faker.Lorem.Paragraph(),
                category.Id,
                startsAtUtc,
                null);

            Result<TicketType> result = TicketType.Create(
                eventResult.Value.Id,
                _faker.Music.Genre(),
                _faker.Random.Decimal(1, 1000000000),
                "USD",
                _faker.Random.Decimal(1, 1000000000));

            var ticketType = result.Value;

            // Act
            ticketType.UpdatePrice(_faker.Random.Decimal(1, 1000000000));

            // Assert
            var domainEvent = AssertDomainEventWasPublished<TicketTypePriceChangedDomainEvent>(ticketType);

            domainEvent.TicketTypeId.Should().Be(ticketType.Id);
        }
    }
}