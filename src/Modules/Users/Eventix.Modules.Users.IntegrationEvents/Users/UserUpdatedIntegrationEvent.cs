using Eventix.Shared.Application.EventBus;

namespace Eventix.Modules.Users.IntegrationEvents.Users
{
    public sealed record UserUpdatedIntegrationEvent : IntegrationEvent
    {
        public UserUpdatedIntegrationEvent(
            Guid correlationId,
            DateTime occurredOnUtc,
            Guid userId,
            string firstName,
            string lastName
            ) : base(correlationId, occurredOnUtc)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid UserId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
    }
}