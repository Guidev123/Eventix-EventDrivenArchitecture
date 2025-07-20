using Eventix.Modules.Attendance.Application.Attendees.UseCases.Create;
using Eventix.Modules.Attendance.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Attendance.IntegrationTests.Attendees.UseCases
{
    public class CreateAttendeeTests : BaseIntegrationTest
    {
        public CreateAttendeeTests(IntegrationWebApplicationFactory factory)
           : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Command Is Invalid")]
        [Trait("Attendance Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new CreateAttendeeCommand(
                Guid.NewGuid(),
                string.Empty,
                string.Empty,
                string.Empty);

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact(DisplayName = "Should Return Success When Command Is Valid")]
        [Trait("Attendance Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateAttendeeCommand(
                Guid.NewGuid(),
                _faker.Internet.Email(),
                _faker.Name.FirstName(),
                _faker.Name.LastName());

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}