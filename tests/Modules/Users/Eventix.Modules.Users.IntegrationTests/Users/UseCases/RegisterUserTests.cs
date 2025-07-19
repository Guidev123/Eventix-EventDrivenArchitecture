using Eventix.Modules.Users.Application.Users.UseCases.Register;
using Eventix.Modules.Users.IntegrationTests.Abstractions;
using FluentAssertions;
using Xunit;

namespace Eventix.Modules.Users.IntegrationTests.Users.UseCases
{
    public class RegisterUserTests : BaseIntegrationTest
    {
        public RegisterUserTests(IntegrationWebApplicationFactory factory) : base(factory)
        {
        }

        public static readonly TheoryData<string, string, string, string, string> InvalidRequests = new()
        {
            { "", _faker.Internet.Password(), _faker.Internet.Password(10), _faker.Name.FirstName(), _faker.Name.LastName() },
            { _faker.Internet.Email(), "", "",  _faker.Name.FirstName(), _faker.Name.LastName() },
            { _faker.Internet.Email(), "12345", "12345", _faker.Name.FirstName(), _faker.Name.LastName() },
            { _faker.Internet.Email(), _faker.Internet.Password(), "Admin@123", "", _faker.Name.LastName() },
            { _faker.Internet.Email(), _faker.Internet.Password(), " ", _faker.Name.FirstName(), "" }
        };

        [Theory(DisplayName = "Should Return Failure When Command Is Not Valid")]
        [Trait("Users Integration Tests", "Use Cases Tests")]
        [MemberData(nameof(InvalidRequests))]
        public async Task Should_ReturnBadRequest_WhenCommandIsNotValid(
            string email,
            string password,
            string confirmPassword,
            string firstName,
            string lastName
            )
        {
            // Arrange
            var command = new RegisterUserCommand(firstName, lastName, email, password, confirmPassword);

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.Error.Should().NotBeNull();
            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
        }

        [Fact(DisplayName = "Should Return Success When Command Is Valid")]
        [Trait("Users Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenCommandIsValid()
        {
            // Arrange
            await _factory.SeedRoleDataAsync();

            var command = new RegisterUserCommand(
                _faker.Person.FirstName,
                _faker.Person.LastName,
                _faker.Internet.Email(),
                "Admin@123",
                "Admin@123"
                );

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.Error.Should().BeNull();
            result.IsFailure.Should().BeFalse();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }
    }
}