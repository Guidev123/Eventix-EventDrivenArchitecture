using Eventix.Modules.Events.Application.Categories.UseCases.Archive;
using Eventix.Modules.Events.Domain.Categories.Errors;
using Eventix.Modules.Events.IntegrationTests.Abstractions;
using Eventix.Shared.Application.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Events.IntegrationTests.Categories.UseCases
{
    public class ArchiveCategoryTests : BaseIntegrationTest
    {
        public ArchiveCategoryTests(IntegrationWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Category Does Not Exist")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenCategoryDoesNotExist()
        {
            // Arrange
            var command = new ArchiveCategoryCommand(Guid.NewGuid());

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.Error.Should().Be(CategoryErrors.NotFound(command.CategoryId));
        }

        [Fact(DisplayName = "Should Archive Category When Category Exists")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ArchiveCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryId = await _mediatorHandler.CreateCategoryAsync(_faker.Music.Genre());

            var command = new ArchiveCategoryCommand(categoryId);

            using var newScope = _factory.Services.CreateScope();
            var mediator = newScope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            // Act
            var result = await mediator.DispatchAsync(command);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact(DisplayName = "Should Return Failure When Category Already Archived")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenCategoryAlreadyArchived()
        {
            // Arrange
            var categoryId = await _mediatorHandler.CreateCategoryAsync(_faker.Music.Genre());

            var command = new ArchiveCategoryCommand(categoryId);

            using var newScope = _factory.Services.CreateScope();
            var mediator = newScope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            await mediator.DispatchAsync(command);

            // Act
            var result = await mediator.DispatchAsync(command);

            // Assert
            result.Error.Should().Be(CategoryErrors.AlreadyArchived);
        }
    }
}