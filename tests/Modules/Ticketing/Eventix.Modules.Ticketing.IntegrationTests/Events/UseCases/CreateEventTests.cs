using Eventix.Modules.Ticketing.Application.Events.UseCases.Create;
using Eventix.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Ticketing.IntegrationTests.Events.UseCases
{
    public class CreateEventTests : BaseIntegrationTest
    {
        public CreateEventTests(IntegrationWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Success When Customer Exists")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenEventIsCreated()
        {
            //Arrange
            var eventId = Guid.NewGuid();
            var ticketTypeId = Guid.NewGuid();
            var quantity = _faker.Random.Decimal(1, 5);

            var ticketType = new CreateEventCommand.TicketTypeRequest(
                ticketTypeId,
                eventId,
                _faker.Music.Genre(),
                _faker.Random.Decimal(1, 1000),
                "USD",
                quantity);

            var command = new CreateEventCommand(
                eventId,
                _faker.Music.Genre(),
                _faker.Lorem.Letter(50),
                null,
                DateTime.UtcNow.AddDays(1),
                null,
                [ticketType]);

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}