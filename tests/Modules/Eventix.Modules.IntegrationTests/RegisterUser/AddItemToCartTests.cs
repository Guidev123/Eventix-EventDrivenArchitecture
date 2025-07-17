using Bogus;
using Eventix.Modules.IntegrationTests.Abstractions;
using Eventix.Modules.Ticketing.Application.Carts.UseCases.AddItem;
using Eventix.Modules.Ticketing.Application.Customers.UseCases.GetById;
using Eventix.Modules.Ticketing.Domain.Orders.ValueObjects;
using Eventix.Modules.Users.Application.Users.UseCases.Register;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;

namespace Eventix.Modules.IntegrationTests.RegisterUser
{
    public class AddItemToCartTests : BaseIntegrationTest
    {
        private const int QUANTITY = 5;

        public AddItemToCartTests(IntegrationWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact(DisplayName = "Customer Should Be Able To Add Item To Cart")]
        [Trait("Modules Integration Tests", "Integration Between Modules Tests")]
        public async Task Customer_ShouldBeAbleTo_AddItemToCart()
        {
            // Arrange
            await _factory.SeedRoleDataAsync();
            var userResult = await _mediatorHandler.DispatchAsync(RegisterUserCommand);

            var customerResult = await Poller.WaitAndExecuteAsync(
                TimeSpan.FromSeconds(15),
                async () =>
                {
                    var query = new GetCustomerByIdQuery(userResult.Value.UserId);

                    var customerResult = await _mediatorHandler.DispatchAsync(query);

                    return customerResult;
                });

            var customer = customerResult.Value;
            var ticketTypeId = Guid.NewGuid();

            await _mediatorHandler.CreateEventAsync(Guid.NewGuid(), ticketTypeId, QUANTITY);

            var command = new AddItemToCartCommand(ticketTypeId, QUANTITY);
            command.SetCustomerId(customer.Id);

            // Act
            var result = await _mediatorHandler.DispatchAsync(command);

            // Assert
            userResult.IsSuccess.Should().BeTrue();
            userResult.IsFailure.Should().BeFalse();
            customerResult.IsSuccess.Should().BeTrue();
            customerResult.IsFailure.Should().BeFalse();
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
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