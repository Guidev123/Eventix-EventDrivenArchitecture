using Eventix.Shared.Application.EventBus;

namespace Eventix.Modules.Users.IntegrationEvents.Users
{
    public sealed record UserUpdatedIntegrationEvent : IntegrationEvent
    {
        public UserUpdatedIntegrationEvent(
            Guid id,
            DateTime occurredOnUtc,
            Guid userId,
            string firstName,
            string lastName
            ) : base(id, occurredOnUtc)
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