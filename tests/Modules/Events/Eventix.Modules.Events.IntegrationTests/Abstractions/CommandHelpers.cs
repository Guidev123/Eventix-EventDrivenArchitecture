using Bogus;
using Eventix.Modules.Events.Application.Categories.UseCases.Create;
using Eventix.Modules.Events.Application.Events.UseCases.Create;
using Eventix.Modules.Events.Application.TicketTypes.UseCases.Create;
using Eventix.Shared.Application.Abstractions;

namespace Eventix.Modules.Events.IntegrationTests.Abstractions
{
    internal static class CommandHelpers
    {
        internal static async Task<Guid> CreateCategoryAsync(this IMediatorHandler mediator, string name)
        {
            return (await mediator.DispatchAsync(new CreateCategoryCommand(name))).Value.CategoryId;
        }

        internal static async Task<Guid> CreateEventAsync(
            this IMediatorHandler mediator,
            Guid categoryId,
            DateTime? startsAtUtc = null)
        {
            var faker = new Faker();
            var result = await mediator.DispatchAsync(
                 new CreateEventCommand(
                     faker.Music.Genre(),
                     faker.Lorem.Letter(50),
                     categoryId,
                     startsAtUtc ?? DateTime.UtcNow.AddDays(10),
                     null));

            return result.Value.Id;
        }

        internal static async Task<Guid> CreateTicketTypeAsync(this IMediatorHandler mediator, Guid eventId)
        {
            var faker = new Faker();
            var result = await mediator.DispatchAsync(
                new CreateTicketTypeCommand(
                    eventId,
                    faker.Commerce.ProductName(),
                    faker.Random.Decimal(1, 1000),
                    "USD",
                    faker.Random.Decimal()));

            return result.Value.Id;
        }
    }
}