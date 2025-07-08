using Dapper;
using Eventix.Modules.Events.Application.TicketTypes.DTOs;
using Eventix.Modules.Events.Domain.TicketTypes.Errors;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetById
{
    internal sealed class GetTicketTypeByIdHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetTicketTypeByIdQuery, TicketTypeResponse>
    {
        public async Task<Result<TicketTypeResponse>> ExecuteAsync(GetTicketTypeByIdQuery request, CancellationToken cancellationToken = default)
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

            var ticketType = await connection.QuerySingleOrDefaultAsync<TicketTypeResponse>(
                sql,
                new { request.TicketTypeId });

            if (ticketType is null)
                return Result.Failure<TicketTypeResponse>(
                    TicketTypeErrors.NotFound(request.TicketTypeId));

            return Result.Success(ticketType);
        }
    }
}