using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.Domain.Events.Interfaces;
using Eventix.Modules.Ticketing.Domain.Payments.Errors;
using Eventix.Modules.Ticketing.Domain.Payments.Interfaces;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Payments.UseCases.RefundForEvent
{
    internal sealed class RefundPaymentsForEventHandler(IEventRepository eventRepository,
                                                        IPaymentRepository paymentRepository,
                                                        ISqlConnectionFactory sqlConnectionFactory) : ICommandHandler<RefundPaymentsForEventCommand>
    {
        public async Task<Result> ExecuteAsync(RefundPaymentsForEventCommand request, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();
            await connection.OpenAsync(cancellationToken);

            using var transaction = await connection.BeginTransactionAsync(cancellationToken);

            try
            {
                var @event = await eventRepository.GetByIdAsync(request.EventId, cancellationToken);
                if (@event is null)
                    return Result.Failure(EventErrors.NotFound(request.EventId));

                var payments = await paymentRepository.GetForEventAsync(@event, cancellationToken);

                foreach (var payment in payments)
                {
                    var reundAmount = payment.AmountRefunded is not null
                        ? payment.AmountRefunded.Amount : decimal.Zero;

                    payment.Refund(payment.Amount.Amount - reundAmount);
                }

                paymentRepository.UpdateRange(payments);

                var saveChanges = await paymentRepository.UnitOfWork.CommitAsync(cancellationToken);
                if (!saveChanges)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    Result.Failure(PaymentErrors.FailToPersistRefundInformation);
                }

                @event.PaymentsRefunded();

                await transaction.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(PaymentErrors.FailToPersistRefundInformation);
            }
        }
    }
}