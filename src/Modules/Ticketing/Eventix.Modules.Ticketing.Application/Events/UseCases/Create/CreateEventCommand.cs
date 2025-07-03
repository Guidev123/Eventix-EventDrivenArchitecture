using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.ValueObjects;
using static Eventix.Modules.Ticketing.Application.Events.UseCases.Create.CreateEventCommand;

namespace Eventix.Modules.Ticketing.Application.Events.UseCases.Create
{
    public sealed record CreateEventCommand(
        Guid EventId,
        string Title,
        string Description,
        Location Location,
        DateTime StartsAtUtc,
        DateTime? EndsAtUtc,
        List<TicketTypeRequest> TicketTypes) : ICommand<CreateEventResponse>
    {
        public sealed record TicketTypeRequest(
            Guid TicketTypeId,
            Guid EventId,
            string Name,
            decimal Price,
            string Currency,
            decimal Quantity);
    }
}