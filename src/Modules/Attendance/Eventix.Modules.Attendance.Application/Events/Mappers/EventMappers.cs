using Eventix.Modules.Attendance.Application.Events.UseCases.Create;
using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Attendance.Application.Events.Mappers
{
    public static class EventMappers
    {
        public static Location? MapToLocation(this CreateEventCommand.LocationRequest? location)
        {
            if (location is null) return null;

            return new Location(
                location.Street,
                location.Number,
                location.AdditionalInfo,
                location.Neighborhood,
                location.ZipCode,
                location.City,
                location.State
            );
        }
    }
}