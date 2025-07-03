using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.Create
{
    public sealed record CreateCustomerCommand(
        Guid CustomerId,
        string Email,
        string FirstName,
        string LastName
        ) : ICommand<CreateCustomerResponse>;
}