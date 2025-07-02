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
                                                        ISqlConnectionFactory sqlConnectionFactory) : ICommandHandler<ArchiveTicketsForEventCommand>
    {
        public async Task<Result> ExecuteAsync(ArchiveTicketsForEventCommand request, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();
            await connection.OpenAsync(cancellationToken);

            using var transaction = await connection.BeginTransactionAsync(cancellationToken);

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

                ticketRepository.UpdateRange(tickets);

                var saveChanges = await ticketRepository.UnitOfWork.CommitAsync(cancellationToken);
                if (!saveChanges)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    Result.Failure(TicketErrors.FailToPersistArchiveTickets);
                }

                @event.TicketsArchived();

                await transaction.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(TicketErrors.FailToPersistArchiveTickets);
            }
        }
    }
}