using Eventix.Modules.Attendance.Application.Events.UseCases.GetStatistics;
using Eventix.Modules.Attendance.Domain.Events.Errors;
using Eventix.Modules.Attendance.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Attendance.IntegrationTests.Events.UseCases
{
    public class GetEventStatisticsTests : BaseIntegrationTest
    {
        public GetEventStatisticsTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Event Statistics Does Not Exist")]
        [Trait("Attendance Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenEventStatisticsDoesNotExist()
        {
            // Arrange
            var query = new GetEventStatisticsQuery(Guid.NewGuid());

            // Act
            var result = await _mediatorHandler.DispatchAsync(query);

            // Assert
            result.Error.Should().Be(EventErrors.StatisticsNotFound(query.EventId));
        }
    }
}