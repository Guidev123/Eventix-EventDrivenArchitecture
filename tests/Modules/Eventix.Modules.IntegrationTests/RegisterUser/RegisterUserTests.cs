using Eventix.Modules.Attendance.Application.Attendees.UseCases.GetById;
using Eventix.Modules.IntegrationTests.Abstractions;
using Eventix.Modules.Ticketing.Application.Customers.UseCases.GetById;
using Eventix.Modules.Users.Application.Users.UseCases.Register;
using FluentAssertions;

namespace Eventix.Modules.IntegrationTests.RegisterUser
{
    public class RegisterUserTests : BaseIntegrationTest
    {
        public RegisterUserTests(IntegrationWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact(DisplayName = "Register User Should Propagate To Ticketing Module")]
        [Trait("Modules Integration Tests", "Integration Between Modules Tests")]
        public async Task RegisterUser_Should_PropagateToTicketingModule()
        {
            // Arrange
            await _factory.SeedRoleDataAsync();

            // Act
            var userResult = await _mediatorHandler.DispatchAsync(RegisterUserCommand);
            var customerResult = await Poller.WaitAndExecuteAsync(TimeSpan.FromSeconds(15), async () =>
            {
                return await _mediatorHandler.DispatchAsync(new GetCustomerByIdQuery(userResult.Value.UserId));
            });

            // Assert
            userResult.IsFailure.Should().BeFalse();
            userResult.IsSuccess.Should().BeTrue();

            customerResult.IsSuccess.Should().BeTrue();
            customerResult.IsSuccess.Should().BeTrue();
            customerResult.Value.Should().NotBeNull();
        }

        [Fact(DisplayName = "Register User Should Propagate To Attendance Module")]
        [Trait("Modules Integration Tests", "Integration Between Modules Tests")]
        public async Task RegisterUser_Should_PropagateToAttendanceModule()
        {
            // Arrange
            await _factory.SeedRoleDataAsync();

            // Act
            var userResult = await _mediatorHandler.DispatchAsync(RegisterUserCommand);
            var attendeeResult = await Poller.WaitAndExecuteAsync(TimeSpan.FromSeconds(15), async () =>
            {
                return await _mediatorHandler.DispatchAsync(new GetAttendeeByIdQuery(userResult.Value.UserId));
            });

            // Assert
            userResult.IsFailure.Should().BeFalse();
            userResult.IsSuccess.Should().BeTrue();

            attendeeResult.IsSuccess.Should().BeTrue();
            attendeeResult.IsSuccess.Should().BeTrue();
            attendeeResult.Value.Should().NotBeNull();
        }

        private static RegisterUserCommand RegisterUserCommand => new(
                _faker.Person.FirstName,
                _faker.Person.LastName,
                _faker.Internet.Email(),
                "Admin@123",
                "Admin@123"
                );
    }
}