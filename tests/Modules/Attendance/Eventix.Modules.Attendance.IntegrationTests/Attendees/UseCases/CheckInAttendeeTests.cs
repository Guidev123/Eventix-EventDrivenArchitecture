using Eventix.Modules.Attendance.Application.Attendees.UseCases.CheckIn;
using Eventix.Modules.Attendance.Domain.Attendees.Errors;
using Eventix.Modules.Attendance.Domain.Tickets.Errors;
using Eventix.Modules.Attendance.IntegrationTests.Abstractions;
using Eventix.Shared.Application.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Attendance.IntegrationTests.Attendees.UseCases
{
    public class CheckInAttendeeTests : BaseIntegrationTest
    {
        public CheckInAttendeeTests(IntegrationWebApplicationFactory factory)
           : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Attendee Does Not Exist")]
        [Trait("Attendance Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenAttendeeDoesNotExist()
        {
            // Arrange
            var command = new CheckInAttendeeCommand(
                Guid.NewGuid(),
                Guid.NewGuid());

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.Error.Should().Be(AttendeeErrors.NotFound(command.AttendeeId));
        }

        [Fact(DisplayName = "Should Return Failure When Ticket Does Not Exist")]
        [Trait("Attendance Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenTicketDoesNotExist()
        {
            // Arrange
            var attendeeId = await _mediatorHandler.CreateAttendeeAsync(Guid.NewGuid());
            var ticketId = Guid.NewGuid();
            var command = new CheckInAttendeeCommand(
                attendeeId,
                ticketId);

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.Error.Should().Be(TicketErrors.NotFound(ticketId));
        }

        [Fact(DisplayName = "Should Return Success When Attendee Checked In")]
        [Trait("Attendance Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenAttendeeCheckedIn()
        {
            //Arrange
            var attendeeId = await _mediatorHandler.CreateAttendeeAsync(Guid.NewGuid());
            var eventId = await _mediatorHandler.CreateEventAsync(Guid.NewGuid());
            var ticketId = await _mediatorHandler.CreateTicketAsync(Guid.NewGuid(), attendeeId, eventId);

            var command = new CheckInAttendeeCommand(
                attendeeId,
                ticketId);

            using var newScope = _factory.Services.CreateScope();
            var mediator = newScope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            //Act
            var result = await mediator.DispatchAsync(command);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}