using Eventix.Modules.Attendance.Application.Events.UseCases.Create;
using Eventix.Modules.Attendance.IntegrationTests.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;

namespace Eventix.Modules.Attendance.IntegrationTests.Events.UseCases
{
    public class CreateEventTests : BaseIntegrationTest
    {
        public CreateEventTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        public static readonly TheoryData<Guid, string, string, DateTime, DateTime?> InvalidData = new()
        {
            { Guid.Empty, _faker.Music.Genre(), _faker.Music.Genre(),  default, default },
            { Guid.NewGuid(), string.Empty, _faker.Music.Genre(), default, default },
            { Guid.NewGuid(), _faker.Music.Genre(), string.Empty,  default, default },
            { Guid.NewGuid(), _faker.Music.Genre(), _faker.Music.Genre(), default, default },
            { Guid.NewGuid(), _faker.Music.Genre(), _faker.Music.Genre(), default, default }
        };

        [Theory(DisplayName = "Should Return Failure When Command Is Invalid")]
        [Trait("Attendance Integration Tests", "Use Cases Tests")]
        [MemberData(nameof(InvalidData))]
        public async Task Should_ReturnFailure_WhenCommandIsInvalid(
            Guid eventId,
            string title,
            string description,
            DateTime startsAtUtc,
            DateTime? endsAtUtc)
        {
            // Arrange
            var command = new CreateEventCommand(eventId, title, description, null, startsAtUtc, endsAtUtc);

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
            var eventId = Guid.NewGuid();

            var command = new CreateEventCommand(
                eventId,
                _faker.Music.Genre(),
                _faker.Lorem.Letter(40),
                null,
                DateTime.UtcNow.AddMinutes(10),
                null);

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}