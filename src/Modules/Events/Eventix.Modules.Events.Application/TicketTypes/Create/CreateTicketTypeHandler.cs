using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Modules.Events.Domain.Shared.Interfaces;
using Eventix.Modules.Events.Domain.TicketTypes.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.TicketTypes.Create
{
    internal sealed class CreateTicketTypeHandler(ITicketTypeRepository ticketTypeRepository,
                                                  IUnitOfWork unitOfWork,
                                                  IEventRepository eventRepository) : ICommandHandler<CreateTicketTypeCommand, CreateTicketTypeResponse>
    {
        public async Task<Result<CreateTicketTypeResponse>> ExecuteAsync(CreateTicketTypeCommand request, CancellationToken cancellationToken = default)
        {
            var @event = eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if (@event is null)
                return Result.Failure<CreateTicketTypeResponse>(EventErrors.NoTicketsFound);

            var ticketType = CreateTicketTypeCommand.ToTicketType(request);

            ticketTypeRepository.Insert(ticketType);
            var rows = await unitOfWork.SaveChangesAsync(cancellationToken);

            return rows > 0
                ? Result.Success(new CreateTicketTypeResponse(ticketType.Id))
                : Result.Failure<CreateTicketTypeResponse>(Error.Problem("TicketTypes.Create", "An error occurred while creating the ticket type."));
        }
    }
}