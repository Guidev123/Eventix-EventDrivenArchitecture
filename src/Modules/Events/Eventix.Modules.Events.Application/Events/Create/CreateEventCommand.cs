using Eventix.Modules.Events.Domain.Events.Entities;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Application.Events.Create
{
    public record CreateEventCommand(
            string Title,
            string Description,
            DateTime StartsAtUtc,
            DateTime? EndsAtUtc
            ) : IRequest<CreateEventResponse>
    {
        public static Event ToEvent(CreateEventCommand command)
            => Event.Create(command.Title, command.Description, command.StartsAtUtc, command.EndsAtUtc);
    }
}