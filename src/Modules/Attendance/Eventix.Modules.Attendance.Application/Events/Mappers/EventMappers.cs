using Eventix.Modules.Attendance.Application.Events.DTOs;
using Eventix.Modules.Attendance.Application.Events.UseCases.Create;
using Eventix.Modules.Attendance.Application.Events.UseCases.GetStatistics;
using Eventix.Modules.Attendance.Domain.Events.Models;
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

        public static LocationResponse MapToLocation(this EventStatisticsRawResponse eventStatistics)
        {
            return new LocationResponse(
                eventStatistics.Street,
                eventStatistics.Number,
                eventStatistics.AdditionalInfo,
                eventStatistics.Neighborhood,
                eventStatistics.ZipCode,
                eventStatistics.City,
                eventStatistics.State
            );
        }

        public static GetEventStatisticsResponse MapToGetEventStatisticsResponse(this EventStatisticsRawResponse eventStatistics,
            LocationResponse location,
            CheckInTicket[] duplicateCheckInTickets,
            CheckInTicket[] invalidCheckInTickets)
        {
            return new GetEventStatisticsResponse(
                eventStatistics.EventId,
                eventStatistics.Title,
                eventStatistics.Description,
                location,
                eventStatistics.StartsAtUtc,
                eventStatistics.EndsAtUtc,
                eventStatistics.TicketsSold,
                eventStatistics.AttendeesCheckedIn,
                duplicateCheckInTickets,
                invalidCheckInTickets);
        }
    }
}