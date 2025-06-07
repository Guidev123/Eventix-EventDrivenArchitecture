using Dapper;
using Eventix.Modules.Events.Application.Abstractions.Data;
using Eventix.Modules.Events.Application.Abstractions.Messaging;
using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Application.Categories.GetAll
{
    internal sealed class GetCategoriesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        : IQueryHandler<GetAllCategoriesQuery, List<GetAllCategoriesResponse>>
    {
        public async Task<Result<List<GetAllCategoriesResponse>>> ExecuteAsync(
            GetAllCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = sqlConnectionFactory.Create();

            const string sql =
                $@"
                SELECT
                Id,
                Name,
                IsArchived
                FROM events.Categories
                ORDER BY Name
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            return Result.Success((await connection.QueryAsync<GetAllCategoriesResponse>(sql, new
            {
                Skip = (request.Page - 1) * request.PageSize,
                Take = request.PageSize
            }
            )).AsList());
        }
    }
}