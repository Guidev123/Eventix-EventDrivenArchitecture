using Eventix.Modules.Users.Application.Users.UseCases.GetPermissions;
using Eventix.Modules.Users.Application.Users.UseCases.Register;
using Eventix.Modules.Users.Domain.Users.Errors;
using Eventix.Modules.Users.IntegrationTests.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Users.IntegrationTests.Users
{
    public class GetUserPermissionTests : BaseIntegrationTest
    {
        public GetUserPermissionTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Error When User Does Not Exist")]
        [Trait("Users Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnError_WhenUserDoesNotExist()
        {
            // Arrange
            var identityId = Guid.NewGuid().ToString();

            // Act
            var permissionsResult = await _mediatorHandler.DispatchAsync(new GetUserPermissionsQuery(identityId));

            // Assert
            permissionsResult.Error.Should().Be(UserErrors.NotFound(identityId));
            permissionsResult.IsFailure.Should().BeTrue();
            permissionsResult.IsSuccess.Should().BeFalse();
        }

        [Fact(DisplayName = "Should Return Permissions When User Exists")]
        [Trait("Users Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnPermissions_WhenUserExists()
        {
            // Arrange
            await _factory.SeedRoleDataAsync();
            var result = await _mediatorHandler.DispatchAsync(new RegisterUserCommand(
                _faker.Name.FirstName(),
                _faker.Name.LastName(),
                _faker.Internet.Email(),
                "Admin@123",
                "Admin@123"));

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == result.Value.UserId)
                ?? throw new Exception();

            var identityId = user.IdentiyProviderId;

            // Act
            var permissionsResult = await _mediatorHandler.DispatchAsync(new GetUserPermissionsQuery(identityId.ToString()));

            // Assert
            permissionsResult.IsSuccess.Should().BeTrue();
            permissionsResult.IsFailure.Should().BeFalse();
            permissionsResult.Value.Permissions.Should().NotBeEmpty();
        }
    }
}