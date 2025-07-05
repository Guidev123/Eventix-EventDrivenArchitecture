using Eventix.Shared.Application.Messaging;
using static Eventix.Modules.Attendance.Application.Events.UseCases.Create.CreateEventCommand;

namespace Eventix.Modules.Attendance.Application.Events.UseCases.Create
{
    public sealed record CreateEventCommand(
        Guid EventId,
        string Title,
        string Description,
        LocationRequest? Location,
        DateTime StartsAtUtc,
        DateTime? EndsAtUtc) : ICommand<CreateEventResponse>
    {
        public sealed record LocationRequest(
            string Street,
            string Number,
            string AdditionalInfo,
            string Neighborhood,
            string ZipCode,
            string City,
            string State
            );
    }
}