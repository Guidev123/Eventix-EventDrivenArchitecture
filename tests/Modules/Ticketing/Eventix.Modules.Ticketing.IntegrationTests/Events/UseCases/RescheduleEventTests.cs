using Eventix.Modules.Ticketing.Application.Events.UseCases.Reschedule;
using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Ticketing.IntegrationTests.Events.UseCases
{
    public class RescheduleEventTests : BaseIntegrationTest
    {
        private const decimal Quantity = 10;

        public RescheduleEventTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Event Does Not Exist")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenEventDoesNotExist()
        {
            //Arrange
            var eventId = Guid.NewGuid();
            var startsAtUtc = DateTime.UtcNow;
            var endsAtUtc = startsAtUtc.AddHours(1);

            var command = new RescheduleEventCommand(eventId, startsAtUtc, endsAtUtc);

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.Error.Should().Be(EventErrors.NotFound(command.EventId));
        }

        [Fact(DisplayName = "Should Return Failure When Event Already Started")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenEventAlreadyStarted()
        {
            //Arrange
            var eventId = Guid.NewGuid();
            var ticketTypeId = Guid.NewGuid();
            var startsAtUtc = DateTime.UtcNow.AddMinutes(-5);
            var endsAtUtc = startsAtUtc.AddHours(1);

            await _mediatorHandler.CreateEventWithTicketTypeAsync(eventId, ticketTypeId, Quantity);

            var command = new RescheduleEventCommand(eventId, startsAtUtc, endsAtUtc);

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.Error.Should().Be(EventErrors.StartDateInPast);
        }

        [Fact(DisplayName = "Should Return Failure When Event Rescheduled")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenEventRescheduled()
        {
            //Arrange
            var eventId = Guid.NewGuid();
            var ticketTypeId = Guid.NewGuid();
            var startsAtUtc = DateTime.UtcNow;
            var endsAtUtc = startsAtUtc.AddHours(1);

            await _mediatorHandler.CreateEventWithTicketTypeAsync(eventId, ticketTypeId, Quantity);

            var command = new RescheduleEventCommand(eventId, startsAtUtc, endsAtUtc);

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.Error.Should().Be(EventErrors.StartDateInPast);
        }
    }
}