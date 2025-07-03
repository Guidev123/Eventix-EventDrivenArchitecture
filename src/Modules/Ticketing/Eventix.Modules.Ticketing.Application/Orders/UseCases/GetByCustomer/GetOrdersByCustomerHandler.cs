using Eventix.Modules.Ticketing.Application.Orders.Mappers;
using Eventix.Modules.Ticketing.Domain.Orders.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Orders.UseCases.GetByCustomer
{
    internal sealed class GetOrdersByCustomerHandler(IOrderRepository orderRepository) : IQueryHandler<GetOrdersByCustomerQuery, List<GetOrdersByCustomerResponse>>
    {
        public async Task<Result<List<GetOrdersByCustomerResponse>>> ExecuteAsync(GetOrdersByCustomerQuery request, CancellationToken cancellationToken = default)
        {
            var orders = await orderRepository.GetAllByCustomerId(request.CustomerId, cancellationToken);
            return Result.Success(orders.Select(x => x.MapToGetOrdersByCustomerResponse()).ToList());
        }
    }
}