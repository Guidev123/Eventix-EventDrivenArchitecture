using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Orders.UseCases.GetById
{
    public record GetOrderByIdQuery(Guid OrderId) : IQuery<GetOrderByIdResponse>;
}