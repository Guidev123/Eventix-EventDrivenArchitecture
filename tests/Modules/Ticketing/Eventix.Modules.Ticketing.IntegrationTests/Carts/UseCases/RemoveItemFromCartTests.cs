using Eventix.Modules.Ticketing.Application.Carts.UseCases.AddItem;
using Eventix.Modules.Ticketing.Application.Carts.UseCases.RemoveItem;
using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.IntegrationTests.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;

namespace Eventix.Modules.Ticketing.IntegrationTests.Carts.UseCases
{
    public class RemoveItemFromCartTests : BaseIntegrationTest
    {
        private const decimal Quantity = 5;

        public RemoveItemFromCartTests(IntegrationWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact(DisplayName = "Should Return Failure When Customer Does Not Exist")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
        {
            //Arrange
            var command = new RemoveItemCommand(Guid.NewGuid());
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

            var command = new RemoveItemCommand(Guid.NewGuid());
            command.SetCustomerId(customerId);

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.Error.Should().Be(TicketTypeErrors.NotFound(command.TicketTypeId));
            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
        }

        [Fact(DisplayName = "Should Return Success When Removed Item From Cart")]
        [Trait("Ticketing Integration Tests", "Use Cases Tests")]
        public async Task Should_ReturnSuccess_WhenRemovedItemFromCart()
        {
            //Arrange
            var customerId = await _mediatorHandler.CreateCustomerAsync(Guid.NewGuid());
            var eventId = Guid.NewGuid();
            var ticketTypeId = Guid.NewGuid();

            await _mediatorHandler.CreateEventWithTicketTypeAsync(eventId, ticketTypeId, Quantity);
            var addItemCommand = new AddItemToCartCommand(ticketTypeId, Quantity);
            addItemCommand.SetCustomerId(customerId);

            await _mediatorHandler.DispatchAsync(addItemCommand);

            var command = new RemoveItemCommand(ticketTypeId);
            command.SetCustomerId(customerId);

            //Act
            var result = await _mediatorHandler.DispatchAsync(command);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }
    }
}