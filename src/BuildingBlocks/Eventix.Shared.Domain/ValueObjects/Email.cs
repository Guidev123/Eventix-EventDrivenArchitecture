using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.ValueObjects.Errors;

namespace Eventix.Shared.Domain.ValueObjects
{
    public record Email : ValueObject
    {
        private const string EMAIL_VALIDATION_PATTERN = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        public Email(string address)
        {
            Address = address;
            Validate();
        }

        private Email()
        { }

        public string Address { get; } = string.Empty;

        public static implicit operator Email(string address) => new(address);

        protected override void Validate()
        {
            AssertionConcern.EnsureMatchesPattern(EMAIL_VALIDATION_PATTERN, Address, ValueObjectErrors.InvalidEmailFormart.Description);
        }
    }
}