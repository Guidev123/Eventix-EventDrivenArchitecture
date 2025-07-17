using Bogus;
using Eventix.Modules.Ticketing.Application.Events.UseCases.Create;
using Eventix.Shared.Application.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.IntegrationTests.Abstractions
{
    internal static class CommandHelpers
    {
        internal static async Task CreateEventAsync(
            this IMediatorHandler mediatorHandler,
            Guid eventId,
            Guid ticketTypeId,
            decimal quantity
            )
        {
            var faker = new Faker();

            var tickeType = new CreateEventCommand.TicketTypeRequest(
                ticketTypeId,
                eventId,
                faker.Music.Genre(),
                faker.Random.Decimal(1, 100000),
                "USD",
                quantity
                );

            var result = await mediatorHandler.DispatchAsync(new CreateEventCommand(
                eventId,
                faker.Music.Genre(),
                faker.Lorem.Letter(50),
                null,
                DateTime.UtcNow.AddDays(1),
                null,
                [tickeType]
                ));

            result.IsSuccess.Should().BeTrue();
        }
    }
}