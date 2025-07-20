using Eventix.Modules.Attendance.Application.Attendees.UseCases.Update;
using Eventix.Modules.Attendance.Domain.Attendees.Errors;
using Eventix.Modules.Attendance.IntegrationTests.Abstractions;
using Eventix.Shared.Application.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Attendance.IntegrationTests.Attendees.UseCases
{
    public class UpdateAttendeeTests : BaseIntegrationTest
    {
        public UpdateAttendeeTests(IntegrationWebApplicationFactory factory)
           : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Attendee Does Not Exist")]
        [Trait("Attendance Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenAttendeeDoesNotExist()
        {
            // Arrange
            var command = new UpdateAttendeeCommand(
                _faker.Name.FirstName(),
                _faker.Name.LastName());
            command.SetAttendeeId(Guid.NewGuid());

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.Error.Should().Be(AttendeeErrors.NotFound(command.AttendeeId));
        }

        [Fact(DisplayName = "Should Return Success When Attendee Exists")]
        [Trait("Attendance Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenAttendeeExists()
        {
            // Arrange
            var attendeeId = await _mediatorHandler.CreateAttendeeAsync(Guid.NewGuid());

            var command = new UpdateAttendeeCommand(
                _faker.Name.FirstName(),
                _faker.Name.LastName());
            command.SetAttendeeId(attendeeId);

            using var newScope = _factory.Services.CreateScope();
            var mediator = newScope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            // Act
            var result = await mediator.DispatchAsync(command);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}