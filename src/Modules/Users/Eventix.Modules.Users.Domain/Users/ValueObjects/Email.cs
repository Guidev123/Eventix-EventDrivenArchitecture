using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Users.Domain.Users.ValueObjects
{
    public record Email : ValueObject
    {
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
            throw new NotImplementedException();
        }
    }
}