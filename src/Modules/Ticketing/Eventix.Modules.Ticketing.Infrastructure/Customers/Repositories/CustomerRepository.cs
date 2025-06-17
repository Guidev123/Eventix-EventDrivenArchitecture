using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;

namespace Eventix.Modules.Ticketing.Infrastructure.Customers.Repositories
{
    internal sealed class CustomerRepository : ICustomerRepository
    {
        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Insert(Customer customer)
        {
        }

        public void Update(Customer customer)
        {
        }

        public void Delete(Customer customer)
        {
        }

        public void Dispose()
        {
        }
    }
}