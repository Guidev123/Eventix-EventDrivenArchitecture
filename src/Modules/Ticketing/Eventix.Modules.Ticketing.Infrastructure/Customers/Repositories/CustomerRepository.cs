using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;
using Eventix.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Ticketing.Infrastructure.Customers.Repositories
{
    internal sealed class CustomerRepository(TicketingDbContext context) : ICustomerRepository
    {
        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Customers.AsNoTracking().AnyAsync(cancellationToken);

        public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public void Insert(Customer customer)
            => context.Customers.Add(customer);

        public void Update(Customer customer)
            => context.Customers.Update(customer);

        public void Delete(Customer customer)
            => context.Customers.Remove(customer);

        public void Dispose()
            => context.Dispose();
    }
}