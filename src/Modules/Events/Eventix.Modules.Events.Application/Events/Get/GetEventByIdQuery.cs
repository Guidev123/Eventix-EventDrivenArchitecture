using Eventix.Modules.Events.Domain.Events.Entities;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Application.Events.Get
{
    public record GetEventByIdQuery(Guid EventId) : IRequest<GetEventByIdResponse?>
    {
        public static GetEventByIdResponse FromEvent(Event @event)
            => new(@event.Id,
                @event.Specification.Title,
                @event.Specification.Description,
                @event.Location, @event.DateRange.StartsAtUtc,
                @event.DateRange.EndsAtUtc, @event.Status);
    }
}