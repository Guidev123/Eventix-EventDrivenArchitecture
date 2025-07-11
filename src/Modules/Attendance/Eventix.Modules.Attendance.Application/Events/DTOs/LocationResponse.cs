namespace Eventix.Modules.Attendance.Application.Events.DTOs
{
    public sealed record LocationResponse(
            string Street,
            string Number,
            string AdditionalInfo,
            string Neighborhood,
            string ZipCode,
            string City,
            string State
            );
}