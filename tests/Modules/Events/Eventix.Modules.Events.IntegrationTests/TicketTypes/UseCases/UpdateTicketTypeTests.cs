using Eventix.Modules.Events.Application.TicketTypes.UseCases.UpdatePrice;
using Eventix.Modules.Events.Domain.TicketTypes.Errors;
using Eventix.Modules.Events.IntegrationTests.Abstractions;
using Eventix.Shared.Application.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Events.IntegrationTests.TicketTypes.UseCases;

public class UpdateTicketTypeTests : BaseIntegrationTest
{
    public UpdateTicketTypeTests(IntegrationWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact(DisplayName = "Should Return Failure When TicketType Does Not Exist")]
    [Trait("Events Integration Tests", "Use Cases Tests")]
    public async Task Should_ReturnFailure_WhenTicketTypeDoesNotExist()
    {
        // Arrange
        var ticketType = Guid.NewGuid();

        var command = new UpdateTicketTypePriceCommand(_faker.Random.Decimal(1, 10000));
        command.SetTicketTypeId(ticketType);

        // Act
        var result = await _mediatorHandler.DispatchAsync(command);

        // Assert
        result.Error.Should().Be(TicketTypeErrors.NotFound(ticketType));
    }

    [Fact(DisplayName = "Should Return Success When TicketType Exists")]
    [Trait("Events Integration Tests", "Use Cases Tests")]
    public async Task Should_ReturnSuccess_WhenTicketTypeExists()
    {
        // Arrange
        var categoryId = await _mediatorHandler.CreateCategoryAsync(_faker.Music.Genre());
        var eventId = await _mediatorHandler.CreateEventAsync(categoryId);
        var ticketTypeId = await _mediatorHandler.CreateTicketTypeAsync(eventId);

        var command = new UpdateTicketTypePriceCommand(_faker.Random.Decimal(1, 10000));
        command.SetTicketTypeId(ticketTypeId);

        using var newScope = _factory.Services.CreateScope();
        var mediator = newScope.ServiceProvider.GetRequiredService<IMediatorHandler>();

        // Act
        var result = await mediator.DispatchAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}