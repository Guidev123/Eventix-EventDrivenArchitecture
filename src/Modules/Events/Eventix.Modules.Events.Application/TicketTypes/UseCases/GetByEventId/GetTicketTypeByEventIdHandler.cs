using Dapper;
using Eventix.Modules.Events.Application.TicketTypes.Dtos;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetByEventId
{
    internal sealed class GetTicketTypeByEventIdHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<GetTicketTypeByEventIdQuery, GetTicketTypeByEventIdResponse>
    {
        public async Task<Result<GetTicketTypeByEventIdResponse>> ExecuteAsync(GetTicketTypeByEventIdQuery request, CancellationToken cancellationToken = default)
        {
            using var connection = connectionFactory.Create();
            const string sql = @"
                SELECT
                    Id,
                    EventId,
                    Name,
                    Amount,
                    Currency,
                    Quantity
                FROM events.TicketTypes
                WHERE EventId = @EventId;";

            var ticketTypes = (await connection.QueryAsync<TicketTypeDto>(sql, new { request.EventId })).ToList();

            return Result.Success(new GetTicketTypeByEventIdResponse(ticketTypes));
        }
    }
}