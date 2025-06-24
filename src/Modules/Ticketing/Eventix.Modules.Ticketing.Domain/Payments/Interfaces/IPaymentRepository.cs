using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Payments.Entities;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Ticketing.Domain.Payments.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Payment>> GetForEventAsync(Event @event, CancellationToken cancellationToken = default);

        void UpdateRange(IEnumerable<Payment> payments);

        void Insert(Payment payment);
    }
}