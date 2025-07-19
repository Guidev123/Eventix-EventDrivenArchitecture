using Eventix.Modules.Events.Application.Categories.UseCases.Create;
using Eventix.Modules.Events.IntegrationTests.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;

namespace Eventix.Modules.Events.IntegrationTests.Categories.UseCases
{
    public class CreateCategoryTests : BaseIntegrationTest
    {
        public CreateCategoryTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Create Category When Command Is Valid")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_CreateCategory_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateCategoryCommand("Category name");

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }

        [Fact(DisplayName = "Should Return Failure When Command Is Not Valid")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenCommandIsNotValid()
        {
            // Arrange
            var command = new CreateCategoryCommand("");

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error?.Type.Should().Be(ErrorTypeEnum.Validation);
        }
    }
}