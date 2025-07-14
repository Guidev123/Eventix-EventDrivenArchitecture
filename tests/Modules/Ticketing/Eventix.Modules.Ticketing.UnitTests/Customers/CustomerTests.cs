using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Modules.Ticketing.UnitTests.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;

namespace Eventix.Modules.Ticketing.UnitTests.Customers;

public class CustomerTests : BaseTest
{
    [Fact(DisplayName = "Create Should Return Value When Customer Is Created")]
    [Trait("Ticketing Unit Tests", "Customer Tests")]
    public void Create_ShouldReturnValue_WhenCustomerIsCreated()
    {
        //Act
        Result<Customer> result = Customer.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Name.FirstName(),
            _faker.Name.LastName());

        //Assert
        result.Value.Should().NotBeNull();
    }
}