using Bogus;
using Eventix.Modules.Events.Application.Events.UseCases.Reschedule;
using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.IntegrationTests.Abstractions;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Events.IntegrationTests.Events.UseCases
{
    public class RescheduleEventTests : BaseIntegrationTest
    {
        public RescheduleEventTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Event Does Not Exist")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            var command = new RescheduleEventCommand(DateTime.UtcNow.AddDays(10), null);
            command.SetEventId(eventId);

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.Error.Should().Be(EventErrors.NotFound(eventId));
            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
        }

        [Fact(DisplayName = "Should Return Failure When Start Date Is In Past")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenStartDateIsInPast()
        {
            // Arrange
            var categoryId = await _mediatorHandler.CreateCategoryAsync(_faker.Music.Genre());
            var eventId = await _mediatorHandler.CreateEventAsync(categoryId);

            var startsAtUtc = DateTime.UtcNow.AddDays(-5);

            var command = new RescheduleEventCommand(startsAtUtc, null);
            command.SetEventId(eventId);

            using var newScope = _factory.Services.CreateScope();
            var mediator = newScope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            // Act
            var result = await mediator.DispatchAsync(command);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
        }

        [Fact(DisplayName = "Should Return Success When Event Is Rescheduled")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenEventIsRescheduled()
        {
            // Arrange
            var categoryId = await _mediatorHandler.CreateCategoryAsync(_faker.Music.Genre());
            var eventId = await _mediatorHandler.CreateEventAsync(categoryId);

            var command = new RescheduleEventCommand(DateTime.UtcNow.AddDays(10), null);
            command.SetEventId(eventId);

            using var newScope = _factory.Services.CreateScope();
            var mediator = newScope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            // Act
            var result = await mediator.DispatchAsync(command);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}