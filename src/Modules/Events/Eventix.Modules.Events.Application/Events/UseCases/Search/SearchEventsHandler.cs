using Dapper;
using Eventix.Modules.Events.Application.Events.UseCases.Get;
using Eventix.Modules.Events.Domain.Events.Enumerators;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;
using System.Data;

namespace Eventix.Modules.Events.Application.Events.UseCases.Search
{
    internal sealed class SearchEventsHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<SearchEventsQuery, SearchEventsResponse>
    {
        public async Task<Result<SearchEventsResponse>> ExecuteAsync(SearchEventsQuery request, CancellationToken cancellationToken = default)
        {
            using var connection = connectionFactory.Create();

            var parameters = new SearchEventsParameters(
            (int)EventStatusEnum.Published,
            request.CategoryId,
            request.StartDate?.Date,
            request.EndDate?.Date,
            request.PageSize,
            (request.Page - 1) * request.PageSize);

            var events = await GetEventsAsync(connection, parameters);

            var totalCount = await GetTotalCountAsync(connection, parameters);

            return Result.Success(new SearchEventsResponse(request.Page, request.PageSize, totalCount, events));
        }

        private static async Task<IReadOnlyCollection<GetEventResponse>> GetEventsAsync(IDbConnection connection, SearchEventsParameters parameters)
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
                WHERE Status = @Status
                AND (@CategoryId IS NULL OR CategoryId = @CategoryId)
                AND (@StartDate IS NULL OR StartsAtUtc >= @StartDate)
                AND (@EndDate IS NULL OR EndsAtUtc <= @EndDate)
                ORDER BY StartsAtUtc
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            return (await connection.QueryAsync<GetEventResponse>(sql, parameters)).AsList();
        }

        private static async Task<int> GetTotalCountAsync(IDbConnection connection, SearchEventsParameters parameters)
        {
            const string sql = @"
                SELECT COUNT(*) FROM events.Events
                WHERE (@CategoryId IS NULL OR CategoryId = @CategoryId)
                AND (@StartDate IS NULL OR StartsAtUtc >= @StartDate)
                AND (@EndDate IS NULL OR EndsAtUtc <= @EndDate);";

            return await connection.ExecuteScalarAsync<int>(sql, parameters);
        }

        private sealed record SearchEventsParameters(
        int Status,
        Guid? CategoryId,
        DateTime? StartDate,
        DateTime? EndDate,
        int Take,
        int Skip);
    }
}