using Eventix.Modules.Users.Domain.Users.DomainEvents;
using Eventix.Modules.Users.Domain.Users.ValueObjects;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Users.Domain.Users.Entities
{
    public sealed class User : Entity, IAggregateRoot
    {
        private User(string email, string firstName, string lastName)
        {
            Email = email;
            Name = (firstName, lastName);
            AuditInfo = (false, DateTime.UtcNow, null);
            Validate();
        }

        private User()
        { }

        public Email Email { get; private set; } = null!;
        public UserName Name { get; private set; } = null!;
        public UserAuditInfo AuditInfo { get; private set; } = null!;

        public static User Create(string email, string firstName, string lastName)
        {
            var user = new User(email, firstName, lastName);

            user.Raise(new UserCreatedDomainEvent(user.Id));

            return user;
        }

        public void UpdateName(string firstName, string lastName)
        {
            if (Name.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase)
                && Name.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)) return;

            var name = (firstName, lastName);
            Name = name;

            Raise(new UserNameUpdatedDomainEvent(Id, Name.FirstName, Name.LastName));
        }

        public void UpdateEmail(string email)
        {
            if (Email.Address.Equals(email,
                StringComparison.OrdinalIgnoreCase)) return;

            Email = email;

            Raise(new UserEmailUpdatedDomainEvent(Id, Email.Address));
        }

        protected override void Validate()
        {
        }
    }
}