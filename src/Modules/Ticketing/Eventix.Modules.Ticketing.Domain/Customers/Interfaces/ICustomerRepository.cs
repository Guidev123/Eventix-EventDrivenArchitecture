using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Ticketing.Domain.Customers.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void Insert(Customer customer);

        void Update(Customer customer);

        void Delete(Customer customer);

        Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}