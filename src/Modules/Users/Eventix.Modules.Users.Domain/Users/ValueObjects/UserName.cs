using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Users.Domain.Users.ValueObjects
{
    public record UserName : ValueObject
    {
        public UserName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Validate();
        }

        private UserName()
        { }

        public string FirstName { get; } = string.Empty;
        public string LastName { get; } = string.Empty;

        public static implicit operator UserName((string firstName, string lastName) name)
            => new(name.firstName, name.lastName);

        protected override void Validate()
        {
        }
    }
}