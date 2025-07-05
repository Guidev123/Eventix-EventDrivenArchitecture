using Eventix.Modules.Attendance.Application.Events.Mappers;
using Eventix.Modules.Attendance.Domain.Events.Entities;
using Eventix.Modules.Attendance.Domain.Events.Errors;
using Eventix.Modules.Attendance.Domain.Events.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Attendance.Application.Events.UseCases.Create
{
    internal sealed class CreateEventHandler(IEventRepository eventRepository) : ICommandHandler<CreateEventCommand, CreateEventResponse>
    {
        public async Task<Result<CreateEventResponse>> ExecuteAsync(CreateEventCommand request, CancellationToken cancellationToken = default)
        {
            var @event = Event.Create(
               request.EventId,
               request.Title,
               request.Description,
               request.StartsAtUtc,
               request.Location.MapToLocation(),
               request.EndsAtUtc);

            eventRepository.Insert(@event);

            var saveChanges = await eventRepository.UnitOfWork.CommitAsync(cancellationToken);

            return saveChanges
                ? Result.Success(new CreateEventResponse(@event.Id))
                : Result.Failure<CreateEventResponse>(EventErrors.FailToCreateEvent);
        }
    }
}