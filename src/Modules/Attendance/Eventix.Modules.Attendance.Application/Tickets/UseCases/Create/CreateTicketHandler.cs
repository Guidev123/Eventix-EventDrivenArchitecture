using Eventix.Modules.Attendance.Domain.Attendees.Errors;
using Eventix.Modules.Attendance.Domain.Attendees.Interfaces;
using Eventix.Modules.Attendance.Domain.Events.Errors;
using Eventix.Modules.Attendance.Domain.Events.Interfaces;
using Eventix.Modules.Attendance.Domain.Tickets.Entities;
using Eventix.Modules.Attendance.Domain.Tickets.Errors;
using Eventix.Modules.Attendance.Domain.Tickets.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Attendance.Application.Tickets.UseCases.Create
{
    internal sealed class CreateTicketHandler(ITicketRepository ticketRepository,
                                              IAttendeeRepository attendeeRepository,
                                              IEventRepository eventRepository) : ICommandHandler<CreateTicketCommand, CreateTicketResponse>
    {
        public async Task<Result<CreateTicketResponse>> ExecuteAsync(CreateTicketCommand request, CancellationToken cancellationToken = default)
        {
            var attendee = await attendeeRepository.GetByIdAsync(request.AttendeeId, cancellationToken);
            if (attendee is null)
                return Result.Failure<CreateTicketResponse>(AttendeeErrors.NotFound(request.AttendeeId));

            var @event = await eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if (@event is null)
                return Result.Failure<CreateTicketResponse>(EventErrors.NotFound(request.EventId));

            var ticket = Ticket.Create(
                request.TicketId,
                attendee.Id,
                @event.Id,
                request.Code);

            ticketRepository.Insert(ticket);

            var saveChanges = await ticketRepository.UnitOfWork.CommitAsync(cancellationToken);
            return saveChanges
                ? Result.Success(new CreateTicketResponse(ticket.Id))
                : Result.Failure<CreateTicketResponse>(TicketErrors.SomethingHasFailedDuringPersistence);
        }
    }
}