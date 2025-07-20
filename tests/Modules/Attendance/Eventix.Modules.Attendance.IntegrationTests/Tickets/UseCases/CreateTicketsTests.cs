using Eventix.Modules.Attendance.Application.Tickets.UseCases.Create;
using Eventix.Modules.Attendance.Domain.Attendees.Errors;
using Eventix.Modules.Attendance.Domain.Events.Errors;
using Eventix.Modules.Attendance.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Attendance.IntegrationTests.Tickets.UseCases
{
    public class CreateTicketsTests : BaseIntegrationTest
    {
        public CreateTicketsTests(IntegrationWebApplicationFactory factory)
           : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Attendee Does Not Exist")]
        [Trait("Attendance Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenAttendeeDoesNotExist()
        {
            // Arrange
            var command = new CreateTicketCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                $"tc_{_faker.Random.String(27)}");

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.Error.Should().Be(AttendeeErrors.NotFound(command.AttendeeId));
        }

        [Fact(DisplayName = "Should Return Failure When Event Does Not Exist")]
        [Trait("Attendance Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenEventDoesNotExist()
        {
            // Arrange
            var attendeeId = await _mediatorHandler.CreateAttendeeAsync(Guid.NewGuid());

            var command = new CreateTicketCommand(
                Guid.NewGuid(),
                attendeeId,
                Guid.NewGuid(),
                $"tc_{_faker.Random.String(27)}");

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.Error.Should().Be(EventErrors.NotFound(command.EventId));
        }

        [Fact(DisplayName = "Should Return Success When Ticket Is Created")]
        [Trait("Attendance Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenTicketIsCreated()
        {
            //Arrange
            var attendeeId = await _mediatorHandler.CreateAttendeeAsync(Guid.NewGuid());
            var eventId = await _mediatorHandler.CreateEventAsync(Guid.NewGuid());

            var command = new CreateTicketCommand(
                Guid.NewGuid(),
                attendeeId,
                eventId,
                $"tc_{_faker.Random.String(27)}");

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}