using Bogus;
using Eventix.Modules.Ticketing.Application.Customers.UseCases.Create;
using Eventix.Modules.Ticketing.Application.Events.UseCases.Create;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;

namespace Eventix.Modules.Ticketing.IntegrationTests.Abstractions
{
    internal static class CommandHelpers
    {
        internal static async Task<Guid> CreateCustomerAsync(this IMediatorHandler mediator, Guid customerId)
        {
            var faker = new Faker();
            Result result = await mediator.DispatchAsync(
                new CreateCustomerCommand(
                    customerId,
                    faker.Internet.Email(),
                    faker.Person.FirstName,
                    faker.Person.LastName));

            result.IsSuccess.Should().BeTrue();

            return customerId;
        }

        internal static async Task CreateEventWithTicketTypeAsync(
            this IMediatorHandler mediator,
            Guid eventId,
            Guid ticketTypeId,
            decimal quantity)
        {
            var faker = new Faker();

            var ticketType = new CreateEventCommand.TicketTypeRequest(
                ticketTypeId,
                eventId,
                faker.Music.Genre(),
                faker.Random.Decimal(1, 10000),
                "USD",
                quantity);

            Result result = await mediator.DispatchAsync(new CreateEventCommand(
                eventId,
                faker.Music.Genre(),
                faker.Lorem.Letter(50),
                null,
                DateTime.UtcNow.AddDays(1),
                null,
                [ticketType]));

            result.IsSuccess.Should().BeTrue();
        }
    }
}