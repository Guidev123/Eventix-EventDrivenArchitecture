using Dapper;
using Eventix.Modules.Events.Application.TicketTypes.GetById;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;
using System.Data;

namespace Eventix.Modules.Events.Application.TicketTypes.GetAll
{
    internal sealed class GetAllTicketTypesHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<GetAllTicketTypesQuery, GetAllTicketTypesResponse>
    {
        public async Task<Result<GetAllTicketTypesResponse>> ExecuteAsync(GetAllTicketTypesQuery request, CancellationToken cancellationToken = default)
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
                ORDER BY Name
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            var ticketsType = await connection.QueryAsync<GetTicketTypeResponse>(sql, new
            {
                Skip = (request.Page - 1) * request.PageSize,
                Take = request.PageSize
            });

            var result = ticketsType.AsList();
            var totalCount = await GetTotalCountAsync(connection);

            return Result.Success(new GetAllTicketTypesResponse(request.Page, request.PageSize, totalCount, result));
        }

        private static async Task<int> GetTotalCountAsync(IDbConnection connection)
        {
            const string sql = @"
                SELECT SUM(s.row_count) FROM sys.dm_db_partition_stats AS s
                WHERE OBJECT_NAME(object_id) = 'TicketTypes'
                AND s.index_id IN (0, 1);";

            return await connection.ExecuteScalarAsync<int>(sql);
        }
    }
}