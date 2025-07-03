using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Orders.UseCases.GetByCustomer
{
    public sealed record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<List<GetOrdersByCustomerResponse>>;
}