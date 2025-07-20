using Bogus;
using Eventix.Modules.Attendance.Application.Attendees.UseCases.Create;
using Eventix.Modules.Attendance.Application.Events.UseCases.Create;
using Eventix.Modules.Attendance.Application.Tickets.UseCases.Create;
using Eventix.Shared.Application.Abstractions;
using FluentAssertions;

namespace Eventix.Modules.Attendance.IntegrationTests.Abstractions
{
    internal static class CommandHelpers
    {
        internal static async Task<Guid> CreateAttendeeAsync(this IMediatorHandler mediator, Guid attendeeId)
        {
            var faker = new Faker();
            var result = await mediator.DispatchAsync(
                new CreateAttendeeCommand(
                    attendeeId,
                    faker.Internet.Email(),
                    faker.Name.FirstName(),
                    faker.Name.LastName()));

            result.IsSuccess.Should().BeTrue();

            return attendeeId;
        }

        internal static async Task<Guid> CreateTicketAsync(
            this IMediatorHandler mediator,
            Guid ticketId,
            Guid attendeeId,
            Guid eventId)
        {
            var faker = new Faker();
            var result = await mediator.DispatchAsync(
                new CreateTicketCommand(
                    ticketId,
                    attendeeId,
                    eventId,
                    $"tc_{faker.Lorem.Letter(27)}"));

            result.IsSuccess.Should().BeTrue();

            return ticketId;
        }

        internal static async Task<Guid> CreateEventAsync(this IMediatorHandler mediator, Guid eventId)
        {
            var faker = new Faker();
            var result = await mediator.DispatchAsync(
                new CreateEventCommand(
                    eventId,
                    faker.Music.Genre(),
                    faker.Lorem.Letter(50),
                    null,
                    DateTime.UtcNow.AddDays(10),
                    null));

            result.IsSuccess.Should().BeTrue();

            return eventId;
        }
    }
}