using Eventix.Modules.Ticketing.Application.Events.UseCases.Cancel;
using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.IntegrationTests.Abstractions;
using Eventix.Shared.Application.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Ticketing.IntegrationTests.Events.UseCases
{
    public class CancelEventTests : BaseIntegrationTest
    {
        private const decimal Quantity = 5;

        public CancelEventTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Event Does Not Exist")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenEventDoesNotExist()
        {
            //Arrange
            var eventId = Guid.NewGuid();

            //Act
            var result = await _mediatorHandler.DispatchAsync(new CancelEventCommand(eventId));

            //Assert
            result.Error.Should().Be(EventErrors.NotFound(eventId));
        }

        [Fact(DisplayName = "Should Return Success When Event Is Canceled")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenEventIsCanceled()
        {
            //Arrange
            var eventId = Guid.NewGuid();
            var ticketTypeId = Guid.NewGuid();

            await _mediatorHandler.CreateEventWithTicketTypeAsync(eventId, ticketTypeId, Quantity);

            var command = new CancelEventCommand(eventId);

            using var newScope = _factory.Services.CreateScope();
            var mediator = newScope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            //Act
            var result = await mediator.DispatchAsync(command);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}