using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.Update
{
    public record UpdateCustomerCommand : ICommand
    {
        public UpdateCustomerCommand(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
        public Guid CustomerId { get; private set; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public void SetCustomerId(Guid customerId) => CustomerId = customerId;
    }
}