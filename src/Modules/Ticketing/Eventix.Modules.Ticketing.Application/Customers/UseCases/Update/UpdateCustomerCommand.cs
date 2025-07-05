using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.Update
{
    public sealed record UpdateCustomerCommand : ICommand
    {
        public UpdateCustomerCommand(
            string firstName,
            string lastName
            )
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid CustomerId { get; private set; }
        public string FirstName { get; }
        public string LastName { get; }
        public void SetCustomerId(Guid customerId) => CustomerId = customerId;
    }
}