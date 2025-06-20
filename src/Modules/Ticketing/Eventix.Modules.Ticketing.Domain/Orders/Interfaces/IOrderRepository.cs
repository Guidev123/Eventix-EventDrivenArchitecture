using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Ticketing.Domain.Orders.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<List<Order>> GetAllByCustomerId(Guid customerId, CancellationToken cancellationToken = default);

        void Insert(Order order);
    }
}