using Eventix.Modules.Ticketing.Application.Customers.UseCases.Create;
using Eventix.Modules.Ticketing.Application.Customers.UseCases.Update;
using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.IntegrationTests.Abstractions;
using Eventix.Shared.Application.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Ticketing.IntegrationTests.Customers.UseCases
{
    public class CreateCustomerTests : BaseIntegrationTest
    {
        public CreateCustomerTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Command Is Invalid")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenCommandIsInvalid()
        {
            //Arrange
            var command = new CreateCustomerCommand(
                Guid.NewGuid(),
                string.Empty,
                string.Empty,
                string.Empty);

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact(DisplayName = "Should Create Customer When Command Is Valid")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_CreateCustomer_WhenCommandIsValid()
        {
            //Arrange
            var command = new CreateCustomerCommand(
                Guid.NewGuid(),
                _faker.Internet.Email(),
                _faker.Name.FirstName(),
                _faker.Name.LastName());

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }
    }

    public class UpdateCustomerTests : BaseIntegrationTest
    {
        public UpdateCustomerTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Customer Does Not Exist")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
        {
            //Arrange
            var command = new UpdateCustomerCommand(
                _faker.Name.FirstName(),
                _faker.Name.LastName());

            command.SetCustomerId(Guid.NewGuid());

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.Error.Should().Be(CustomerErrors.NotFound(command.CustomerId));
        }

        [Fact(DisplayName = "Should Return Success When Customer Is Updated")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenCustomerIsUpdated()
        {
            //Arrange
            var customerId = await _mediatorHandler.CreateCustomerAsync(Guid.NewGuid());

            var command = new UpdateCustomerCommand(_faker.Name.FirstName(), _faker.Name.LastName());
            command.SetCustomerId(customerId);

            using var newScope = _factory.Services.CreateScope();
            var mediator = newScope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            //Act
            var result = await mediator.DispatchAsync(command);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}