using Dapper;
using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.Events.UseCases.GetById
{
    internal sealed class GetEventByIdHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<GetEventByIdQuery, GetEventResponse>
    {
        public async Task<Result<GetEventResponse>> ExecuteAsync(GetEventByIdQuery request, CancellationToken cancellationToken = default)
        {
            const string sql = $@"
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
                    FROM events.Events WHERE Id = @Id";

            using var connection = connectionFactory.Create();

            var eventData = await connection.QuerySingleOrDefaultAsync<GetEventResponse>(sql, new { Id = request.EventId }).ConfigureAwait(false);

            if (eventData is null)
                return Result.Failure<GetEventResponse>(EventErrors.NotFound(request.EventId));

            return Result.Success(eventData);
        }
    }
}