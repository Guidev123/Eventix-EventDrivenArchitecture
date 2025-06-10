using Dapper;
using Eventix.Modules.Events.Application.Categories.UseCases.GetById;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;
using System.Data;

namespace Eventix.Modules.Events.Application.Categories.UseCases.GetAll
{
    internal sealed class GetCategoriesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
            : IQueryHandler<GetAllCategoriesQuery, GetAllCategoriesResponse>
    {
        public async Task<Result<GetAllCategoriesResponse>> ExecuteAsync(
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

            var totalCount = await GetTotalCountAsync(connection);

            var categories = await connection.QueryAsync<GetCategoryResponse>(sql, new
            {
                Skip = (request.Page - 1) * request.PageSize,
                Take = request.PageSize
            });

            var result = categories.ToList().AsReadOnly();

            return Result.Success(new GetAllCategoriesResponse(request.Page, request.PageSize, totalCount, result));
        }

        private static async Task<int> GetTotalCountAsync(IDbConnection connection)
        {
            const string sql = @"
                    SELECT SUM(s.row_count) FROM sys.dm_db_partition_stats AS s
                    WHERE OBJECT_NAME(object_id) = 'Categories'
                    AND s.index_id IN (0, 1);";

            return await connection.ExecuteScalarAsync<int>(sql);
        }
    }
}