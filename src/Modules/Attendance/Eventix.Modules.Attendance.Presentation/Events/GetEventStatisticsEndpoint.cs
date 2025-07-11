using Eventix.Modules.Attendance.Application.Events.UseCases.GetStatistics;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Attendance.Presentation.Events
{
    internal sealed class GetEventStatisticsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/attendance/events/{id:guid}/statistics", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetEventStatisticsQuery(id));

                return result.Match(() => Results.Ok(result.Value), ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetEventStatistics).WithTags(Tags.Attendance);
        }
    }
}