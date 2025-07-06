using Eventix.Modules.Events.Application.Events.UseCases.AttachLocation;
using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Events.Application.Events.Mappers
{
    public static class EventMappers
    {
        public static Location MapToLocation(this AttachEventLocationCommand command)
            => new(
                command.Street,
                command.Number,
                command.AdditionalInfo,
                command.Neighborhood,
                command.ZipCode,
                command.City,
                command.State);
    }
}