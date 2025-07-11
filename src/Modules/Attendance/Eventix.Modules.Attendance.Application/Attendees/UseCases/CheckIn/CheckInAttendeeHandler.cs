using Eventix.Modules.Attendance.Domain.Attendees.Errors;
using Eventix.Modules.Attendance.Domain.Attendees.Interfaces;
using Eventix.Modules.Attendance.Domain.Tickets.Errors;
using Eventix.Modules.Attendance.Domain.Tickets.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;
using Microsoft.Extensions.Logging;

namespace Eventix.Modules.Attendance.Application.Attendees.UseCases.CheckIn
{
    internal sealed class CheckInAttendeeHandler(IAttendeeRepository attendeeRepository,
                                                 ILogger<CheckInAttendeeHandler> logger,
                                                 ITicketRepository ticketRepository) : ICommandHandler<CheckInAttendeeCommand>
    {
        public async Task<Result> ExecuteAsync(CheckInAttendeeCommand request, CancellationToken cancellationToken = default)
        {
            var attendee = await attendeeRepository.GetByIdAsync(request.AttendeeId, cancellationToken);
            if (attendee is null)
                return Result.Failure(AttendeeErrors.NotFound(request.AttendeeId));

            var ticket = await ticketRepository.GetByIdAsync(request.TicketId, cancellationToken);
            if (ticket is null)
                return Result.Failure(TicketErrors.NotFound(request.TicketId));

            var result = attendee.CheckIn(ticket);

            ticketRepository.Update(ticket);

            await attendeeRepository.UnitOfWork.CommitAsync(cancellationToken);
            await ticketRepository.UnitOfWork.CommitAsync(cancellationToken);

            if (result.IsFailure)
            {
                logger.LogWarning("Check in failed: {AttendeeId}, {TicketId}, {@Error}",
                    attendee.Id, ticket.Id, result.Error);
            }

            return result;
        }
    }
}