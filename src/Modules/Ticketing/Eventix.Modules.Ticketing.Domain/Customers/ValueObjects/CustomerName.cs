using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Ticketing.Domain.Customers.ValueObjects
{
    public record CustomerName : ValueObject
    {
        private const int NAME_MAX_LENGTH = 50;

        public CustomerName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Validate();
        }

        private CustomerName()
        { }

        public string FirstName { get; } = string.Empty;
        public string LastName { get; } = string.Empty;

        public static implicit operator CustomerName((string firstName, string lastName) name)
            => new(name.firstName, name.lastName);

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(FirstName, CustomerErrors.CustomerNameMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotEmpty(LastName, CustomerErrors.CustomerNameMustBeNotEmpty.Description);
            AssertionConcern.EnsureTrue(FirstName.Length <= NAME_MAX_LENGTH, CustomerErrors.CustomerNameLengthMustNotExceedTheLimitCharacters(NAME_MAX_LENGTH).Description);
            AssertionConcern.EnsureTrue(LastName.Length <= NAME_MAX_LENGTH, CustomerErrors.CustomerNameLengthMustNotExceedTheLimitCharacters(NAME_MAX_LENGTH).Description);
        }
    }
}