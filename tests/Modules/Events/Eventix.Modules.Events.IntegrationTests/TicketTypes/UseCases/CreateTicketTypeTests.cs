using Eventix.Modules.Events.Application.TicketTypes.UseCases.Create;
using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Events.IntegrationTests.TicketTypes.UseCases;

public class CreateTicketTypeTests : BaseIntegrationTest
{
    public CreateTicketTypeTests(IntegrationWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact(DisplayName = "Should Return Failure When Event Does Not Exist")]
    [Trait("Events Integration Tests", "Use Cases Tests")]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange
        var command = new CreateTicketTypeCommand(
            Guid.NewGuid(),
            _faker.Commerce.ProductName(),
            _faker.Random.Decimal(1, 1000),
            "USD",
            _faker.Random.Decimal(1, 5));

        // Act
        var result = await _mediatorHandler.DispatchAsync(command);

        // Assert
        result.Error.Should().Be(EventErrors.NotFound(command.EventId));
    }
}