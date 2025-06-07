using Dapper;
using Eventix.Modules.Events.Application.Abstractions.Data;
using Eventix.Modules.Events.Application.Abstractions.Messaging;
using Eventix.Modules.Events.Domain.Categories.Errors;
using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Application.Categories.GetById
{
    public sealed class GetCategoryByIdHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdResponse>
    {
        public async Task<Result<GetCategoryByIdResponse>> ExecuteAsync(GetCategoryByIdQuery request, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();

            const string sql = $@"
                    SELECT
                    Name,
                    IsArchived
                    FROM events.Categories
                    WHERE Id = @CategoryId";

            var category = await connection.QuerySingleOrDefaultAsync<GetCategoryByIdResponse>(sql, new { request.CategoryId });
            if (category is null)
                return Result.Failure<GetCategoryByIdResponse>(CategoryErrors.NotFound(request.CategoryId));

            return Result.Success(category);
        }
    }
}