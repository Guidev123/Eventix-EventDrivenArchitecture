using Eventix.Modules.Users.Application.Users.UseCases.GetById;
using Eventix.Modules.Users.Application.Users.UseCases.Register;
using Eventix.Modules.Users.Domain.Users.Errors;
using Eventix.Modules.Users.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Users.IntegrationTests.Users.UseCases
{
    public class GetUserByIdTests : BaseIntegrationTest
    {
        public GetUserByIdTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Error When User Does Not Exist")]
        [Trait("Users Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnError_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var userResult = await _mediatorHandler.DispatchAsync(new GetUserByIdQuery(userId));

            // Assert
            userResult.Error.Should().Be(UserErrors.NotFound(userId));
        }

        [Fact(DisplayName = "Should Return User When User Exist")]
        [Trait("Users Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnUser_WhenUserExist()
        {
            // Arrange
            await _factory.SeedRoleDataAsync();
            var command = new RegisterUserCommand(
                _faker.Person.FirstName,
                _faker.Person.LastName,
                _faker.Person.Email,
                "Admin@123",
                "Admin@123");

            var createUserResult = await _mediatorHandler.DispatchAsync(command);

            // Act
            var userResult = await _mediatorHandler.DispatchAsync(new GetUserByIdQuery(createUserResult.Value.UserId));

            // Assert
            userResult.Value.Id.Should().Be(createUserResult.Value.UserId);
            userResult.Error.Should().BeNull();
        }
    }
}