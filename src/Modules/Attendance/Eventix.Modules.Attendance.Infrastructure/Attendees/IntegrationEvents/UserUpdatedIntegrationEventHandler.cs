using Eventix.Modules.Attendance.Application.Attendees.UseCases.Update;
using Eventix.Modules.Users.IntegrationEvents.Users;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;

namespace Eventix.Modules.Attendance.Infrastructure.Attendees.IntegrationEvents
{
    internal sealed class UserUpdatedIntegrationEventHandler(IMediatorHandler mediator) : IntegrationEventHandler<UserUpdatedIntegrationEvent>
    {
        public override async Task ExecuteAsync(UserUpdatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var command = new UpdateAttendeeCommand(
                integrationEvent.FirstName,
                integrationEvent.LastName
            );

            command.SetAttendeeId(integrationEvent.UserId);

            var result = await mediator.DispatchAsync(command, cancellationToken);

            if (result.IsFailure)
                throw new EventixException(nameof(UpdateAttendeeCommand), result.Error);
        }
    }
}