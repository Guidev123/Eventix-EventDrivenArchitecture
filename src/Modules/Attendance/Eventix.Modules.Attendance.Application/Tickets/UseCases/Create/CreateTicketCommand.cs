using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Attendance.Application.Tickets.UseCases.Create
{
    public sealed record CreateTicketCommand(
        Guid TicketId,
        Guid AttendeeId,
        Guid EventId,
        string Code
        ) : ICommand<CreateTicketResponse>;
}