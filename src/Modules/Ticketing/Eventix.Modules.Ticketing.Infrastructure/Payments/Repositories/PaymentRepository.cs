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

        public async Task<IEnumerable<Payment>> GetForEventAsync(Event @event, CancellationToken cancellationToken = default)
            => await (from order in context.Orders
                      join payment in context.Payments on order.Id equals payment.OrderId
                      join orderItem in context.OrderItems on order.Id equals orderItem.OrderId
                      join ticketType in context.TicketTypes on orderItem.TicketTypeId equals ticketType.Id
                      where ticketType.EventId == @event.Id
                      select payment).ToListAsync(cancellationToken).ConfigureAwait(false);

        public void Insert(Payment payment)
            => context.Payments.Add(payment);

        public void UpdateRange(IEnumerable<Payment> payments)
            => context.Payments.UpdateRange(payments);

        public void Dispose() => context.Dispose();
    }
}