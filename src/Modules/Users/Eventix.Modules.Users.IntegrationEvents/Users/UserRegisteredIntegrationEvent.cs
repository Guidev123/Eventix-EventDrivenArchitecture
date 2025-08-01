﻿using Eventix.Shared.Application.EventBus;

namespace Eventix.Modules.Users.IntegrationEvents.Users
{
    public record UserRegisteredIntegrationEvent : IntegrationEvent
    {
        public UserRegisteredIntegrationEvent(Guid correlationId, DateTime occurredOnUtc, Guid userId, string email, string firstName, string lastName) : base(correlationId, occurredOnUtc)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid UserId { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}