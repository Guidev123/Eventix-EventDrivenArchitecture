using Dapper;
using Eventix.Modules.Events.Application.Abstractions.Data;
using Eventix.Modules.Events.Application.Abstractions.Messaging;
using Eventix.Modules.Events.Domain.Shared;
using Eventix.Modules.Events.Domain.TicketTypes.Errors;

namespace Eventix.Modules.Events.Application.TicketTypes.GetById
{
    internal sealed class GetTicketTypeByIdHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetTicketTypeByIdQuery, GetTicketTypeResponse>
    {
        public async Task<Result<GetTicketTypeResponse>> ExecuteAsync(GetTicketTypeByIdQuery request, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();

            const string sql = $@"
                SELECT
                    Id,
                    EventId,
                    Name,
                    Amount,
                    Currency,
                    Quantity
                FROM events.TicketTypes
                WHERE Id = @TicketTypeId;";

            var ticketType = await connection.QuerySingleOrDefaultAsync<GetTicketTypeResponse>(
                sql,
                new { request.TicketTypeId });

            if (ticketType is null)
                return Result.Failure<GetTicketTypeResponse>(
                    TicketTypeErrors.NotFound(request.TicketTypeId));

            return Result.Success(ticketType);
        }
    }
}