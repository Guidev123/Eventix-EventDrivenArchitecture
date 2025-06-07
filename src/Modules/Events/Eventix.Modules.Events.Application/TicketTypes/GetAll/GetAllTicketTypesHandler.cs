using Dapper;
using Eventix.Modules.Events.Application.Abstractions.Data;
using Eventix.Modules.Events.Application.Abstractions.Messaging;
using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Application.TicketTypes.GetAll
{
    internal sealed class GetAllTicketTypesHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<GetAllTicketTypesQuery, List<GetAllTicketTypesResponse>>
    {
        public async Task<Result<List<GetAllTicketTypesResponse>>> ExecuteAsync(GetAllTicketTypesQuery request, CancellationToken cancellationToken = default)
        {
            using var connection = connectionFactory.Create();

            const string sql = @"
                SELECT
                Id,
                EventId,
                Name,
                Price,
                Currency,
                Quantity
                FROM events.TicketTypes
                ORDER BY Name
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            return Result.Success((await connection.QueryAsync<GetAllTicketTypesResponse>(sql, new
            {
                Skip = request.Page,
                Take = (request.Page - 1) * request.PageSize
            }
            )).AsList());
        }
    }
}