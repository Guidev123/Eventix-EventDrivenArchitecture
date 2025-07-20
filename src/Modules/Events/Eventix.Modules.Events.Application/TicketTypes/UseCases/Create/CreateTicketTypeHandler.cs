using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Modules.Events.Domain.TicketTypes.Errors;
using Eventix.Modules.Events.Domain.TicketTypes.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.Create
{
    internal sealed class CreateTicketTypeHandler(ITicketTypeRepository ticketTypeRepository,
                                                  IEventRepository eventRepository) : ICommandHandler<CreateTicketTypeCommand, CreateTicketTypeResponse>
    {
        public async Task<Result<CreateTicketTypeResponse>> ExecuteAsync(CreateTicketTypeCommand request, CancellationToken cancellationToken = default)
        {
            var @event = await eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if (@event is null)
                return Result.Failure<CreateTicketTypeResponse>(EventErrors.NotFound(request.EventId));

            var ticketType = CreateTicketTypeCommand.ToTicketType(request);

            ticketTypeRepository.Insert(ticketType);

            var saveChanges = await ticketTypeRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges
                ? Result.Success(new CreateTicketTypeResponse(ticketType.Id))
                : Result.Failure<CreateTicketTypeResponse>(TicketTypeErrors.FailToCreateTicket);
        }
    }
}