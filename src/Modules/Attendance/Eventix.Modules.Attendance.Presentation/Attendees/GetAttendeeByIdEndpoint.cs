using Eventix.Modules.Attendance.Application.Attendees.UseCases.GetById;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Attendance.Presentation.Attendees
{
    internal sealed class GetAttendeeByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/attendees/{id:guid}", async (Guid id, IMediatorHandler mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetAttendeeByIdQuery(id));

                return result.Match(() => Results.Ok(result.Value), ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetTickets).WithTags(Tags.Attendance);
        }
    }
}