using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Modules.Ticketing.Domain.Payments.DomainEvents;
using Eventix.Modules.Ticketing.Domain.Payments.Entities;
using Eventix.Modules.Ticketing.Domain.Payments.Errors;
using Eventix.Modules.Ticketing.UnitTests.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;

namespace Eventix.Modules.Ticketing.UnitTests.Payments;

public class PaymentTests : BaseTest
{
    [Fact(DisplayName = "Create Should Raise Domain Event When Payment Is Created")]
    [Trait("Ticketing Unit Tests", "Payment Tests")]
    public void Create_ShouldRaiseDomainEvent_WhenPaymentIsCreated()
    {
        // Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Name.FirstName(),
            _faker.Name.LastName());

        var order = Order.Create(customer);

        // Act
        Result<Payment> result = Payment.Create(
            order,
            Guid.NewGuid(),
            _faker.Random.Decimal(1, 1000000000),
            "USD");

        // Assert
        var domainEvent =
            AssertDomainEventWasPublished<PaymentCreatedDomainEvent>(result.Value);

        domainEvent.PaymentId.Should().Be(result.Value.Id);
    }

    [Fact(DisplayName = "Refund Should Return Failure When Already Refunded")]
    [Trait("Ticketing Unit Tests", "Payment Tests")]
    public void Refund_ShouldReturnFailure_WhenAlreadyRefunded()
    {
        // Arrange
        var amount = _faker.Random.Decimal(1, 1000000000);

        var customer = Customer.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Name.FirstName(),
            _faker.Name.LastName());

        var order = Order.Create(customer);

        Result<Payment> paymentResult = Payment.Create(
            order,
            Guid.NewGuid(),
            amount,
            "USD");

        Payment payment = paymentResult.Value;

        payment.Refund(amount);

        // Act
        Result result = payment.Refund(amount);

        // Assert
        result.Error.Should().Be(PaymentErrors.AlreadyRefunded);
    }
}