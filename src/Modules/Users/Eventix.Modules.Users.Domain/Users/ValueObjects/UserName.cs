using Eventix.Modules.Users.Domain.Users.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Users.Domain.Users.ValueObjects
{
    public record UserName : ValueObject
    {
        private const int NAME_MAX_LENGTH = 50;

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
            AssertionConcern.EnsureNotEmpty(FirstName, UserErrors.UserNameMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotEmpty(LastName, UserErrors.UserNameMustBeNotEmpty.Description);
            AssertionConcern.EnsureTrue(FirstName.Length <= NAME_MAX_LENGTH, UserErrors.UserNameLengthMustNotExceedTheLimitCharacters(NAME_MAX_LENGTH).Description);
            AssertionConcern.EnsureTrue(LastName.Length <= NAME_MAX_LENGTH, UserErrors.UserNameLengthMustNotExceedTheLimitCharacters(NAME_MAX_LENGTH).Description);
        }
    }
}