using Eventix.Modules.Ticketing.Application.Carts.Errors;
using Eventix.Modules.Ticketing.Application.Carts.UseCases.AddItem;
using Eventix.Modules.Ticketing.Application.Orders.UseCases.Create;
using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Ticketing.IntegrationTests.Orders.UseCases
{
    public class CreateOrderTests : BaseIntegrationTest
    {
        private const decimal Quantity = 5;

        public CreateOrderTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Success When Customer Exists")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenCustomerExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var eventId = Guid.NewGuid();
            var ticketId = Guid.NewGuid();

            await _mediatorHandler.CreateCustomerAsync(customerId);
            await _mediatorHandler.CreateEventWithTicketTypeAsync(eventId, ticketId, Quantity);

            var addItemToCartCommand = new AddItemToCartCommand(ticketId, Quantity);
            addItemToCartCommand.SetCustomerId(customerId);

            await _mediatorHandler.DispatchAsync(addItemToCartCommand);

            var command = new CreateOrderCommand(customerId);

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Error.Should().BeNull();
        }

        [Fact(DisplayName = "Should Return Failure When Customer Does Not Exist")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
        {
            //Arrange
            var command = new CreateOrderCommand(Guid.NewGuid());

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.Error.Should().Be(CustomerErrors.NotFound(command.CustomerId));
        }

        [Fact(DisplayName = "Should Return Failure When Cart Is Empty")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenCartIsEmpty()
        {
            //Arrange
            var customerId = await _mediatorHandler.CreateCustomerAsync(Guid.NewGuid());

            var command = new CreateOrderCommand(customerId);

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.Error.Should().Be(CartErrors.Empty);
        }
    }
}