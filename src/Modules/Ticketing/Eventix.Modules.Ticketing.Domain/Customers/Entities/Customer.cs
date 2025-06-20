using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Users.Domain.Users.ValueObjects;
using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Ticketing.Domain.Customers.Entities
{
    public sealed class Customer : Entity, IAggregateRoot
    {
        private Customer(Guid id, string email, string firstName, string lastName)
        {
            Id = id;
            Email = email;
            Name = (firstName, lastName);
            Validate();
        }

        private Customer()
        { }

        public Email Email { get; private set; } = null!;
        public Name Name { get; private set; } = null!;

        public static Customer Create(Guid id, string email, string firstName, string lastName)
        {
            var customer = new Customer(id, email, firstName, lastName);

            return customer;
        }

        public void UpdateName(string firstName, string lastName)
        {
            if (Name.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase)
                && Name.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)) return;

            var name = (firstName, lastName);
            Name = name;
        }

        public void UpdateEmail(string email)
        {
            if (Email.Address.Equals(email,
                StringComparison.OrdinalIgnoreCase)) return;

            Email = email;
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotNull(Email, CustomerErrors.EmailMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotNull(Name, CustomerErrors.NameMustBeNotEmpty.Description);
        }
    }
}