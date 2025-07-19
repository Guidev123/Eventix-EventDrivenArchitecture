using Eventix.Modules.Events.Application.Categories.UseCases.Update;
using Eventix.Modules.Events.Domain.Categories.Errors;
using Eventix.Modules.Events.IntegrationTests.Abstractions;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Events.IntegrationTests.Categories.UseCases
{
    public class UpdateCategoryTests : BaseIntegrationTest
    {
        public UpdateCategoryTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        public static readonly TheoryData<UpdateCategoryCommand> InvalidCommands = new()
        {
            new UpdateCategoryCommand(_faker.Music.Genre()),
            new UpdateCategoryCommand(string.Empty)
        };

        [Theory(DisplayName = "Should Return Failure When Command Is Not Valid")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        [MemberData(nameof(InvalidCommands))]
        public async Task Should_ReturnFailure_WhenCommandIsNotValid(UpdateCategoryCommand command)
        {
            // Act
            command.SetCategoryId(Guid.Empty);
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error?.Type.Should().Be(ErrorTypeEnum.Validation);
        }

        [Fact(DisplayName = "Should Return Failure When Category Does Not Exist")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var command = new UpdateCategoryCommand(_faker.Music.Genre());
            command.SetCategoryId(categoryId);

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.Error.Should().Be(CategoryErrors.NotFound(categoryId));
        }

        [Fact(DisplayName = "Should Update Category When Category Exists")]
        [Trait("Events Integration Tests", "Use Cases Tests")]
        public async Task Should_UpdateCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryId = await _mediatorHandler.CreateCategoryAsync(_faker.Music.Genre());

            var command = new UpdateCategoryCommand(_faker.Music.Genre());
            command.SetCategoryId(categoryId);

            using var newScope = _factory.Services.CreateScope();
            var mediator = newScope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            // Act
            var result = await mediator.DispatchAsync(command);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}