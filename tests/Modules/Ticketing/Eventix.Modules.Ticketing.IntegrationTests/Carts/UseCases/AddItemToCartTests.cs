using Eventix.Modules.Ticketing.Application.Carts.UseCases.AddItem;
using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Ticketing.IntegrationTests.Carts.UseCases
{
    public class AddItemToCartTests : BaseIntegrationTest
    {
        private const decimal Quantity = 5;

        public AddItemToCartTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Success When Item Added To Cart")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenItemAddedToCart()
        {
            //Arrange
            var customerId = await _mediatorHandler.CreateCustomerAsync(Guid.NewGuid());
            var eventId = Guid.NewGuid();
            var ticketTypeId = Guid.NewGuid();

            await _mediatorHandler.CreateEventWithTicketTypeAsync(eventId, ticketTypeId, Quantity);

            var command = new AddItemToCartCommand(ticketTypeId, Quantity);
            command.SetCustomerId(customerId);

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Error.Should().BeNull();
        }

        [Fact(DisplayName = "Should Return Failure When Customer Does Not Exist")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
        {
            //Arrange
            var command = new AddItemToCartCommand(Guid.NewGuid(), Quantity);
            command.SetCustomerId(Guid.NewGuid());

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.Error.Should().Be(CustomerErrors.NotFound(command.CustomerId));
            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
        }

        [Fact(DisplayName = "Should Return Failure When TicketType Does Not Exist")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenTicketTypeDoesNotExist()
        {
            //Arrange
            var customerId = await _mediatorHandler.CreateCustomerAsync(Guid.NewGuid());

            var command = new AddItemToCartCommand(Guid.NewGuid(), Quantity);
            command.SetCustomerId(customerId);

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.Error.Should().Be(TicketTypeErrors.NotFound(command.TicketTypeId));
            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
        }

        [Fact(DisplayName = "Should Return Failure When Not Enough Quantity")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenNotEnoughQuantity()
        {
            //Arrange
            var customerId = await _mediatorHandler.CreateCustomerAsync(Guid.NewGuid());
            var eventId = Guid.NewGuid();
            var ticketTypeId = Guid.NewGuid();

            await _mediatorHandler.CreateEventWithTicketTypeAsync(eventId, ticketTypeId, Quantity);

            var command = new AddItemToCartCommand(ticketTypeId, Quantity + 1);
            command.SetCustomerId(customerId);

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.Error.Should().Be(TicketTypeErrors.NotEnoughQuantity(Quantity));
            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
        }
    }
}