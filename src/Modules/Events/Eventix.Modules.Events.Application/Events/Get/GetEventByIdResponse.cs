using Eventix.Modules.Events.Domain.Events.Enumerators;
using Eventix.Modules.Events.Domain.Events.ValueObjects;

namespace Eventix.Modules.Events.Application.Events.Get
{
    public record GetEventByIdResponse(
        Guid Id,
        string Title,
        string Description,
        Location? Location,
        DateTime StartsAtUtc,
        DateTime? EndsAtUtc,
        EventStatusEnum Status
        );
}