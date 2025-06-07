using Dapper;
using Eventix.Modules.Events.Application.Abstractions.Data;
using Eventix.Modules.Events.Application.Abstractions.Messaging;
using Eventix.Modules.Events.Domain.Categories.Errors;
using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Application.Categories.GetById
{
    public sealed class GetCategoryByIdHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetCategoryByIdQuery, GetCategoryResponse>
    {
        public async Task<Result<GetCategoryResponse>> ExecuteAsync(GetCategoryByIdQuery request, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();

            const string sql = $@"
                    SELECT
                    Id,
                    Name,
                    IsArchived
                    FROM events.Categories
                    WHERE Id = @CategoryId";

            var category = await connection.QuerySingleOrDefaultAsync<GetCategoryResponse>(sql, new { request.CategoryId });
            if (category is null)
                return Result.Failure<GetCategoryResponse>(CategoryErrors.NotFound(request.CategoryId));

            return Result.Success(category);
        }
    }
}