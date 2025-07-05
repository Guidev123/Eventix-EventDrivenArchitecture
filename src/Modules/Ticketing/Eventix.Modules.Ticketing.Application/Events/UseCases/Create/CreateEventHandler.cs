using Eventix.Modules.Ticketing.Application.Events.Mappers;
using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.Domain.Events.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Events.UseCases.Create
{
    internal sealed class CreateEventHandler(
            IEventRepository eventRepository,
            ITicketTypeRepository ticketTypeRepository) : ICommandHandler<CreateEventCommand, CreateEventResponse>
    {
        public async Task<Result<CreateEventResponse>> ExecuteAsync(CreateEventCommand request, CancellationToken cancellationToken = default)
        {
            var eventResult = Event.Create(
                request.EventId,
                request.Title,
                request.Description,
                request.StartsAtUtc,
                request.Location.MapToLocation(),
                request.EndsAtUtc);

            if (eventResult.Error is not null
                && eventResult.IsFailure)
                return Result.Failure<CreateEventResponse>(eventResult.Error);

            var @event = eventResult.Value;

            eventRepository.Insert(@event);

            IEnumerable<TicketType> ticketTypes = request.TicketTypes
                .Select(t => TicketType.Create(t.TicketTypeId, t.EventId, t.Name, t.Price, t.Currency, t.Quantity));

            ticketTypeRepository.InsertRange(ticketTypes);

            var saveChangesEvents = await eventRepository.UnitOfWork.CommitAsync(cancellationToken);

            return saveChangesEvents
                ? Result.Success(new CreateEventResponse(@event.Id))
                : Result.Failure<CreateEventResponse>(EventErrors.FailToCreateEvent);
        }
    }
}