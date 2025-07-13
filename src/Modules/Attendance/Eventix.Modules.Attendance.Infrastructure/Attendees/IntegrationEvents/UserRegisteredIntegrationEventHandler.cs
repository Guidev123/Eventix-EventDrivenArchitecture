using Eventix.Modules.Attendance.Application.Attendees.UseCases.Create;
using Eventix.Modules.Users.IntegrationEvents.Users;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;

namespace Eventix.Modules.Attendance.Infrastructure.Attendees.IntegrationEvents
{
    internal sealed class UserRegisteredIntegrationEventHandler(IMediatorHandler mediator) : IntegrationEventHandler<UserRegisteredIntegrationEvent>
    {
        public override async Task ExecuteAsync(UserRegisteredIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var command = new CreateAttendeeCommand(
                integrationEvent.UserId,
                integrationEvent.Email,
                integrationEvent.FirstName,
                integrationEvent.LastName
                );

            var result = await mediator.DispatchAsync(command, cancellationToken);

            if (result.IsFailure)
                throw new EventixException(nameof(CreateAttendeeCommand), result.Error);
        }
    }
}