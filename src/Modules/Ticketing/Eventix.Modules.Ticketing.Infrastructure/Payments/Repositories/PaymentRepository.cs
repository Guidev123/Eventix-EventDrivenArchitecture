using Eventix.Modules.Ticketing.Domain.Events.Entities;
using Eventix.Modules.Ticketing.Domain.Payments.Entities;
using Eventix.Modules.Ticketing.Domain.Payments.Interfaces;
using Eventix.Modules.Ticketing.Infrastructure.Database;
using Eventix.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Ticketing.Infrastructure.Payments.Repositories
{
    internal sealed class PaymentRepository(TicketingDbContext context) : IPaymentRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Payments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public void Update(Payment payment)
            => context.Payments.Update(payment);

        public void Insert(Payment payment)
            => context.Payments.Add(payment);

        public void Dispose() => context.Dispose();
    }
}