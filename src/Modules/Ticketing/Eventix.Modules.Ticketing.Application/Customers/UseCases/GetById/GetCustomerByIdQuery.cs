using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.GetById
{
    public sealed record GetCustomerByIdQuery(Guid CustomerId) : IQuery<GetCustomerByIdResponse>;
}