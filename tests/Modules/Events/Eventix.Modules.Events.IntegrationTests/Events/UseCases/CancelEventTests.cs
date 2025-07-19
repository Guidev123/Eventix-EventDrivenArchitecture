using Eventix.Modules.Events.Application.Events.UseCases.Cancel;
using Eventix.Modules.Events.Application.Events.UseCases.Publish;
using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.IntegrationTests.Abstractions;
using Eventix.Shared.Application.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Eventix.Modules.Events.IntegrationTests.Events.UseCases
{
    public class CancelEventTests : BaseIntegrationTest
    {
        public CancelEventTests(IntegrationWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Event Does Not Exist")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            var command = new CancelEventCommand(eventId);

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.Error.Should().Be(EventErrors.NotFound(eventId));
        }

        [Fact(DisplayName = "Should Return Failure When Event Already Canceled")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenEventAlreadyCanceled()
        {
            // Arrange
            var categoryId = await _mediatorHandler.CreateCategoryAsync(_faker.Music.Genre());
            var eventId = await _mediatorHandler.CreateEventAsync(categoryId);

            var command = new CancelEventCommand(eventId);

            using var newScope = _factory.Services.CreateScope();
            var mediator = newScope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            await mediator.DispatchAsync(command);

            using var newScope2 = _factory.Services.CreateScope();
            var mediator2 = newScope2.ServiceProvider.GetRequiredService<IMediatorHandler>();

            // Act
            var result = await mediator2.DispatchAsync(command);

            // Assert
            result.Error.Should().Be(EventErrors.AlreadyCanceled);
        }

        [Fact(DisplayName = "Should Return Success When Event Is Canceled")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenEventIsCanceled()
        {
            // Arrange
            var categoryId = await _mediatorHandler.CreateCategoryAsync(_faker.Music.Genre());
            var eventId = await _mediatorHandler.CreateEventAsync(categoryId);

            var command = new CancelEventCommand(eventId);

            using var newScope = _factory.Services.CreateScope();
            var mediator = newScope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            // Act
            var result = await mediator.DispatchAsync(command);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}