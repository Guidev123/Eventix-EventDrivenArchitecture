using Eventix.Modules.Events.Application.Categories.UseCases.GetAll;
using Eventix.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Events.IntegrationTests.Categories.UseCases
{
    public class GetAllCategoriesTests : BaseIntegrationTest
    {
        public GetAllCategoriesTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Empty Collection When No Categories Exist")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnEmptyCollection_WhenNoCategoriesExist()
        {
            // Arrange
            await CleanDatabaseAsync();

            var query = new GetAllCategoriesQuery(1, 50);

            // Act
            var result = await _mediatorHandler.DispatchAsync(query);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Categories.Count.Should().Be(0);
        }

        [Fact(DisplayName = "Should Return Category When Category Exists")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnCategory_WhenCategoryExists()
        {
            // Arrange
            await CleanDatabaseAsync();

            await _mediatorHandler.CreateCategoryAsync(_faker.Music.Genre());
            await _mediatorHandler.CreateCategoryAsync(_faker.Music.Genre());

            var query = new GetAllCategoriesQuery(1, 50);

            // Act
            var result = await _mediatorHandler.DispatchAsync(query);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.TotalCount.Should().Be(2);
        }
    }
}