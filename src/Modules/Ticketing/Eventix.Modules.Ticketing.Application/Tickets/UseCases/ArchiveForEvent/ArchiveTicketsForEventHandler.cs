using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.Domain.Events.Interfaces;
using Eventix.Modules.Ticketing.Domain.Tickets.Errors;
using Eventix.Modules.Ticketing.Domain.Tickets.Interfaces;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Tickets.UseCases.ArchiveForEvent
{
    internal sealed class ArchiveTicketsForEventHandler(IEventRepository eventRepository,
                                                        ITicketRepository ticketRepository,
                                                        IUnitOfWork unitOfWork,
                                                        ISqlConnectionFactory sqlConnectionFactory) : ICommandHandler<ArchiveTicketsForEventCommand>
    {
        public async Task<Result> ExecuteAsync(ArchiveTicketsForEventCommand request, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();
            var transaction = await connection.BeginTransactionAsync(cancellationToken);

            try
            {
                var @event = await eventRepository.GetByIdAsync(request.EventId, cancellationToken);
                if (@event is null)
                    return Result.Failure(EventErrors.NotFound(request.EventId));

                var tickets = await ticketRepository.GetForEventAsync(@event, cancellationToken);

                foreach (var ticket in tickets)
                {
                    ticket.Archive();
                }

                @event.TicketsArchived();

                var saveChanges = await unitOfWork.CommitAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return saveChanges
                    ? Result.Success()
                    : Result.Failure(TicketErrors.FailToPersistArchiveTickets);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(TicketErrors.FailToPersistArchiveTickets);
            }
        }
    }
}