using Dapper;
using Eventix.Modules.Events.Application.Abstractions.Data;
using Eventix.Modules.Events.Application.Abstractions.Messaging;
using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Application.Events.Get
{
    public sealed class GetEventByIdHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<GetEventByIdQuery, GetEventByIdResponse>
    {
        public async Task<Result<GetEventByIdResponse>> ExecuteAsync(GetEventByIdQuery request, CancellationToken cancellationToken = default)
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
                    Status
                    FROM events.Events WHERE Id = @Id";

            using var connection = connectionFactory.Create();

            var eventData = await connection.QuerySingleOrDefaultAsync<GetEventByIdResponse>(sql, new { Id = request.EventId }).ConfigureAwait(false);

            if (eventData is null)
                return Result.Failure<GetEventByIdResponse>(EventErrors.NotFound(request.EventId));

            return Result.Success(eventData);
        }
    }
}