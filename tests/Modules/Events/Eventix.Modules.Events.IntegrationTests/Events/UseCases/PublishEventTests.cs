using Eventix.Modules.Events.Application.Events.UseCases.AttachLocation;
using Eventix.Modules.Events.Application.Events.UseCases.Publish;
using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.IntegrationTests.Abstractions;
using Eventix.Shared.Application.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Events.IntegrationTests.Events.UseCases
{
    public class PublishEventTests : BaseIntegrationTest
    {
        public PublishEventTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Event Does Not Exist")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            var command = new PublishEventCommand(eventId);

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.Error.Should().Be(EventErrors.NotFound(eventId));
        }
    }
}