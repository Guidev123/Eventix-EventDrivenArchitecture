using Eventix.Modules.Attendance.Application.Attendees.UseCases.CheckIn;
using Eventix.Shared.Presentation.Endpoints;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MidR.Interfaces;

namespace Eventix.Modules.Attendance.Presentation.Attendees
{
    internal sealed class CheckInAttendeeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/attendees/check-in", async (CheckInAttendeeCommand command, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(command);

                return result.Match(Results.NoContent, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.CheckInTicket).WithTags(Tags.Attendance);
        }
    }
}