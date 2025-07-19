using Eventix.Modules.Users.Application.Users.UseCases.Register;
using Eventix.Modules.Users.Application.Users.UseCases.Update;
using Eventix.Modules.Users.Domain.Users.Errors;
using Eventix.Modules.Users.IntegrationTests.Abstractions;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Users.IntegrationTests.Users.UseCases
{
    public class UpdateUserTests : BaseIntegrationTest
    {
        public UpdateUserTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        public static readonly TheoryData<UpdateUserCommand> InvalidCommands =
        [
            new UpdateUserCommand(string.Empty, string.Empty),
            new UpdateUserCommand(string.Empty, _faker.Name.LastName()),
            new UpdateUserCommand(_faker.Name.FirstName(), string.Empty)
        ];

        [Theory(DisplayName = "Should Return Error When Command Is Not Valid")]
        [Trait("Users Integration Tests", "Use Cases Tests")]
        [MemberData(nameof(InvalidCommands))]
        public async Task Should_ReturnError_WhenCommandIsNotValid(UpdateUserCommand command)
        {
            // Arrange
            command.SetUserId(Guid.NewGuid());

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error?.Type.Should().Be(ErrorTypeEnum.Validation);
        }

        [Fact(DisplayName = "Should Return Error When User Does Not Exist")]
        [Trait("Users Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnError_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new UpdateUserCommand(_faker.Name.FirstName(), _faker.Name.LastName());
            command.SetUserId(userId);

            // Act
            var updateResult = await _mediatorHandler.DispatchAsync(command);

            // Assert
            updateResult.Error.Should().Be(UserErrors.NotFound(userId));
            updateResult.IsFailure.Should().BeTrue();
            updateResult.IsSuccess.Should().BeFalse();
        }

        [Fact(DisplayName = "Should Return Success When User Exists")]
        [Trait("Users Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenUserExists()
        {
            // Arrange
            await _factory.SeedRoleDataAsync();

            var result = await _mediatorHandler.DispatchAsync(new RegisterUserCommand(
                _faker.Name.FirstName(),
                _faker.Name.LastName(),
                _faker.Internet.Email(),
                "Admin@123",
                "Admin@123"
            ));

            var userId = result.Value.UserId;

            using var newScope = _factory.Services.CreateScope();
            var mediator = newScope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            var command = new UpdateUserCommand(_faker.Name.FirstName(), _faker.Name.LastName());
            command.SetUserId(userId);

            // Act
            var updateResult = await mediator.DispatchAsync(command);

            // Assert
            updateResult.IsSuccess.Should().BeTrue();
            updateResult.IsFailure.Should().BeFalse();
        }
    }
}