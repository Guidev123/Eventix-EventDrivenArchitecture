using Eventix.Modules.Ticketing.Application.Orders.Mappers;
using Eventix.Modules.Ticketing.Domain.Orders.Errors;
using Eventix.Modules.Ticketing.Domain.Orders.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Orders.UseCases.GetById
{
    internal sealed class GetOrderByIdHandler(IOrderRepository orderRepository) : IQueryHandler<GetOrderByIdQuery, GetOrderByIdResponse>
    {
        public async Task<Result<GetOrderByIdResponse>> ExecuteAsync(GetOrderByIdQuery request, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            return order is null
                ? Result.Failure<GetOrderByIdResponse>(OrderErrors.NotFound(request.OrderId))
                : Result.Success(order.MapToGetOrderByIdResponse());
        }
    }
}