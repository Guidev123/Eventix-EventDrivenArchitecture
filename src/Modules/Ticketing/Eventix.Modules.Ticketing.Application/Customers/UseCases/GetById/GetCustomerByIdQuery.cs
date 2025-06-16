using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.GetById
{
    public record GetCustomerByIdQuery(Guid CustomerId) : IQuery<GetCustomerByIdResponse>;
}