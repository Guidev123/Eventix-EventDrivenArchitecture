using Dapper;
using Eventix.Modules.Events.Application.TicketTypes.Dtos;
using Eventix.Modules.Events.Domain.TicketTypes.Errors;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.GetById
{
    internal sealed class GetTicketTypeByIdHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetTicketTypeByIdQuery, TicketTypeDto>
    {
        public async Task<Result<TicketTypeDto>> ExecuteAsync(GetTicketTypeByIdQuery request, CancellationToken cancellationToken = default)
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

            var ticketType = await connection.QuerySingleOrDefaultAsync<TicketTypeDto>(
                sql,
                new { request.TicketTypeId });

            if (ticketType is null)
                return Result.Failure<TicketTypeDto>(
                    TicketTypeErrors.NotFound(request.TicketTypeId));

            return Result.Success(ticketType);
        }
    }
}