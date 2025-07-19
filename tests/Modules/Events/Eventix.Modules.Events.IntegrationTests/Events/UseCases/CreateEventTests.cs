using Eventix.Modules.Events.Application.Events.UseCases.Create;
using Eventix.Modules.Events.Domain.Categories.Errors;
using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.IntegrationTests.Abstractions;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Domain.Responses;
using Eventix.Shared.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Events.IntegrationTests.Events.UseCases
{
    public class CreateEventTests : BaseIntegrationTest
    {
        public CreateEventTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Start Date In Past")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenStartDateInPast()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-1);

            var command = new CreateEventCommand(
                _faker.Music.Genre(),
                _faker.Lorem.Letter(50),
                Guid.NewGuid(),
                startDate,
                null);

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
        }

        [Fact(DisplayName = "Should Return Failure When Category Does Not Exist")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            var command = new CreateEventCommand(
                _faker.Music.Genre(),
                _faker.Lorem.Letter(43),
                categoryId,
                DateTime.UtcNow.AddDays(1),
                null);

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.Error.Should().Be(CategoryErrors.NotFound(categoryId));
        }

        [Fact(DisplayName = "Should Return Failure When EndDate Precedes StartDate")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenEndDatePrecedesStartDate()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var startsAtUtc = DateTime.UtcNow.AddMinutes(10);
            var endsAtUtc = startsAtUtc.AddMinutes(-5);

            var command = new CreateEventCommand(
                _faker.Music.Genre(),
                _faker.Music.Genre(),
                Guid.NewGuid(),
                startsAtUtc,
                endsAtUtc);

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.Error?.Type.Should().Be(ErrorTypeEnum.Validation);
        }

        [Fact(DisplayName = "Should Create Event When Command Is Valid")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_CreateEvent_WhenCommandIsValid()
        {
            // Arrange
            await CleanDatabaseAsync();
            var categoryId = await _mediatorHandler.CreateCategoryAsync(_faker.Music.Genre());

            var command = new CreateEventCommand(
                _faker.Music.Genre(),
                _faker.Lorem.Letter(50),
                categoryId,
                DateTime.UtcNow.AddDays(1),
                null);

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }
    }
}