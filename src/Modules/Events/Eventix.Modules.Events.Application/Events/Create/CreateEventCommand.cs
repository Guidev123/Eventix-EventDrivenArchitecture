using Eventix.Modules.Events.Application.Abstractions.Messaging;
using Eventix.Modules.Events.Domain.Events.Entities;

namespace Eventix.Modules.Events.Application.Events.Create
{
    public record CreateEventCommand(
            string Title,
            string Description,
            DateTime StartsAtUtc,
            DateTime? EndsAtUtc
            ) : ICommand<CreateEventResponse>
    {
        public static Event ToEvent(CreateEventCommand command)
            => Event.Create(command.Title, command.Description, command.StartsAtUtc, command.EndsAtUtc).Value;
    }
}