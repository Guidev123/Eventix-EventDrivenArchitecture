using Azure.Core;
using Dapper;
using Eventix.Modules.Events.Application.Events.Get;
using Eventix.Shared.Application.Data;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;
using System.Data;

namespace Eventix.Modules.Events.Application.Events.GetAll
{
    public sealed class GetAllEventsHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<GetAllEventsQuery, GetAllEventsResponse>
    {
        public async Task<Result<GetAllEventsResponse>> ExecuteAsync(GetAllEventsQuery request, CancellationToken cancellationToken = default)
        {
            using var connection = connectionFactory.Create();

            var events = await GetEventsAsync(connection, request);
            var count = await GetTotalCountAsync(connection);

            return Result.Success(new GetAllEventsResponse(request.Page, request.PageSize, count, events));
        }

        private static async Task<int> GetTotalCountAsync(IDbConnection connection)
        {
            const string sql = @"
                SELECT SUM(s.row_count) FROM sys.dm_db_partition_stats AS s
                WHERE OBJECT_NAME(object_id) = 'Events'
                AND s.index_id IN (0, 1);";

            return await connection.ExecuteScalarAsync<int>(sql);
        }

        private static async Task<IReadOnlyCollection<GetEventResponse>> GetEventsAsync(IDbConnection connection, GetAllEventsQuery request)
        {
            const string sql = @"
                SELECT
                    Id,
                    Title,
                    Description,
                    Street,
                    City,
                    State,
                    ZipCode,
                    Number,
                    AdditionalInfo,
                    Neighborhood,
                    StartsAtUtc,
                    EndsAtUtc,
                    Status,
                    CategoryId
                FROM events.Events
                ORDER BY StartsAtUtc
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            return (await connection.QueryAsync<GetEventResponse>(sql, new
            {
                Skip = (request.Page - 1) * request.PageSize,
                Take = request.PageSize
            })).AsList();
        }
    }
}