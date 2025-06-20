using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Orders.UseCases.GetByCustomer
{
    public record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<List<GetOrdersByCustomerResponse>>;
}