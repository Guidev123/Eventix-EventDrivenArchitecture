using Eventix.Modules.Ticketing.Application.Carts.UseCases.Clear;
using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Ticketing.IntegrationTests.Carts.UseCases
{
    public class ClearCartTests : BaseIntegrationTest
    {
        public ClearCartTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Customer Does Not Exist")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
        {
            //Arrange
            var command = new ClearCartCommand(Guid.NewGuid());

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.Error.Should().Be(CustomerErrors.NotFound(command.CustomerId));
            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
        }

        [Fact(DisplayName = "Should Return Success When Customer Exists")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenCustomerExists()
        {
            //Arrange
            Guid customerId = await _mediatorHandler.CreateCustomerAsync(Guid.NewGuid());

            var command = new ClearCartCommand(customerId);

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }
    }
}