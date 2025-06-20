using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.ValueObjects.Errors;

namespace Eventix.Modules.Users.Domain.Users.ValueObjects
{
    public record Name : ValueObject
    {
        public const int NAME_MAX_LENGTH = 50;
        public const int NAME_MIN_LENGTH = 2;
        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Validate();
        }

        private Name()
        { }

        public string FirstName { get; } = string.Empty;
        public string LastName { get; } = string.Empty;

        public static implicit operator Name((string firstName, string lastName) name)
            => new(name.firstName, name.lastName);

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(FirstName, ValueObjectErrors.NameMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotEmpty(LastName, ValueObjectErrors.NameMustBeNotEmpty.Description);
            AssertionConcern.EnsureTrue(FirstName.Length <= NAME_MAX_LENGTH, ValueObjectErrors.NameLengthMustNotExceedTheLimitCharacters(NAME_MAX_LENGTH).Description);
            AssertionConcern.EnsureTrue(LastName.Length <= NAME_MAX_LENGTH, ValueObjectErrors.NameLengthMustNotExceedTheLimitCharacters(NAME_MAX_LENGTH).Description);
        }
    }
}