using Eventix.Modules.Events.Domain.Events.Entities;
using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.UseCases.Create
{
    public sealed record CreateEventCommand(
            string Title,
            string Description,
            Guid CategoryId,
            DateTime StartsAtUtc,
            DateTime? EndsAtUtc
            ) : ICommand<CreateEventResponse>
    {
        public static Event ToEvent(CreateEventCommand command)
            => Event.Create(command.Title, command.Description, command.CategoryId, command.StartsAtUtc, command.EndsAtUtc).Value;
    }
}