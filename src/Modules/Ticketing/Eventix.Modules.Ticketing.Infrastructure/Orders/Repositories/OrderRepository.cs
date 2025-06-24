using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Modules.Ticketing.Domain.Orders.Interfaces;
using Eventix.Modules.Ticketing.Infrastructure.Database;
using Eventix.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Ticketing.Infrastructure.Orders.Repositories
{
    internal sealed class OrderRepository(TicketingDbContext context) : IOrderRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public async Task<List<Order>> GetAllByCustomerId(Guid customerId, CancellationToken cancellationToken = default)
            => await context.Orders.Include(c => c.OrderItems).AsNoTrackingWithIdentityResolution().ToListAsync(cancellationToken);

        public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Orders.Include(c => c.OrderItems).AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public void Insert(Order order)
            => context.Orders.Add(order);

        public void Dispose() => context.Dispose();
    }
}