using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.Domain.Events.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.TicketTypes.UseCases.UpdatePrice
{
    internal sealed class UpdateTicketTypePriceHandler(ITicketTypeRepository ticketTypeRepository,
                                                       IUnitOfWork unitOfWork) : ICommandHandler<UpdateTicketTypePriceCommand>
    {
        public async Task<Result> ExecuteAsync(UpdateTicketTypePriceCommand request, CancellationToken cancellationToken = default)
        {
            var ticketType = await ticketTypeRepository.GetByIdAsync(request.TicketTypeId, cancellationToken);
            if (ticketType is null)
                return Result.Failure(TicketTypeErrors.NotFound(request.TicketTypeId));

            ticketType.UpdatePrice(request.Price);
            ticketTypeRepository.Update(ticketType);

            var saveChanges = await unitOfWork.CommitAsync(cancellationToken);

            return saveChanges
                ? Result.Success()
                : Result.Failure(TicketTypeErrors.UnableToUpdate(ticketType.Specification.Name, ticketType.Id));
        }
    }
}