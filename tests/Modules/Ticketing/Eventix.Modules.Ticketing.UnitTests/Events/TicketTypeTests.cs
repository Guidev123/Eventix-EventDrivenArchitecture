using Eventix.Modules.Ticketing.Domain.Events.DomainEvents;
using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.UnitTests.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;

namespace Eventix.Modules.Ticketing.UnitTests.Events
{
    public class TicketTypeTests : BaseTest
    {
        [Fact(DisplayName = "Create Should Return Value When TicketType Is Created")]
        [Trait("Ticketing Unit Tests", "TicketType Tests")]
        public void Create_ShouldReturnValue_WhenTicketTypeIsCreated()
        {
            // Arrange
            var startsAtUtc = DateTime.UtcNow.AddDays(1);
            var @event = Event.Create(
                Guid.NewGuid(),
                _faker.Music.Genre(),
                _faker.Lorem.Paragraph(),
                startsAtUtc,
                null);

            // Act
            Result<TicketType> result = TicketType.Create(
                Guid.NewGuid(),
                @event.Value.Id,
                _faker.Name.FirstName(),
                _faker.Random.Decimal(1, 1000000000),
                "USD",
                _faker.Random.Decimal(1, 1000000000));

            // Assert
            result.Value.Should().NotBeNull();
        }

        [Fact(DisplayName = "UpdateQuantity Should Return Failure When Not Enough Quantity")]
        [Trait("Ticketing Unit Tests", "TicketType Tests")]
        public void UpdateQuantity_ShouldReturnFailure_WhenNotEnoughQuanitity()
        {
            // Arrange
            var startsAtUtc = DateTime.UtcNow.AddDays(1);
            var @event = Event.Create(
                Guid.NewGuid(),
                _faker.Music.Genre(),
                _faker.Lorem.Paragraph(),
                startsAtUtc,
                null);

            decimal quantity = _faker.Random.Decimal(1, 1000000000);
            var ticketType = TicketType.Create(
                Guid.NewGuid(),
                @event.Value.Id,
                _faker.Name.FirstName(),
                _faker.Random.Decimal(1, 1000000000),
                "USD",
                quantity);

            // Act
            Result result = ticketType.UpdateQuantity(quantity + 1);

            // Assert
            result.Error.Should().Be(TicketTypeErrors.NotEnoughQuantity(quantity));
        }

        [Fact(DisplayName = "UpdateQuantity Should Raise Domain Event When TicketType Is Sold Out")]
        [Trait("Ticketing Unit Tests", "TicketType Tests")]
        public void UpdateQuantity_ShouldRaiseDomainEvent_WhenTicketTypesIsSoldOut()
        {
            // Arrange
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
            ticketType.Value.UpdateQuantity(quantity);

            // Assert
            var domainEvent = AssertDomainEventWasPublished<TicketTypeSoldOutDomainEvent>(ticketType.Value);
            domainEvent.TicketTypeId.Should().Be(ticketType.Value.Id);
        }
    }
}