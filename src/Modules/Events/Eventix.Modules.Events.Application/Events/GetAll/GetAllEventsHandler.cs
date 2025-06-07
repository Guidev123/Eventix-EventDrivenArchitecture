using Dapper;
using Eventix.Modules.Events.Application.Abstractions.Data;
using Eventix.Modules.Events.Application.Abstractions.Messaging;
using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Application.Events.GetAll
{
    public sealed class GetAllEventsHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<GetAllEventsQuery, List<GetAllEventsResponse>>
    {
        public async Task<Result<List<GetAllEventsResponse>>> ExecuteAsync(GetAllEventsQuery request, CancellationToken cancellationToken = default)
        {
            using var connection = connectionFactory.Create();

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
                    Status
                FROM events.Events
                ORDER BY StartsAtUtc
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            return Result.Success((await connection.QueryAsync<GetAllEventsResponse>(sql, new
            {
                Skip = request.Page,
                Take = (request.Page - 1) * request.PageSize
            }
            )).AsList());
        }
    }
}