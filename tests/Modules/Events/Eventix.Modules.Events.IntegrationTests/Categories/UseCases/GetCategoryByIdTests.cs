using Eventix.Modules.Events.Application.Categories.UseCases.GetById;
using Eventix.Modules.Events.Domain.Categories.Errors;
using Eventix.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Events.IntegrationTests.Categories.UseCases
{
    public class GetCategoryByIdTests : BaseIntegrationTest
    {
        public GetCategoryByIdTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Category Does Not Exist")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenCategoryDoesNotExist()
        {
            // Arrange
            var query = new GetCategoryByIdQuery(Guid.NewGuid());

            // Act
            var result = await _mediatorHandler.DispatchAsync(query);

            // Assert
            result.Error.Should().Be(CategoryErrors.NotFound(query.CategoryId));
        }

        [Fact(DisplayName = "Should Return Category When Category Exists")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryId = await _mediatorHandler.CreateCategoryAsync(_faker.Music.Genre());

            var query = new GetCategoryByIdQuery(categoryId);

            // Act
            var result = await _mediatorHandler.DispatchAsync(query);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }
    }
}