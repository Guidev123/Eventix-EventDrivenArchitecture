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
                                                        IUnitOfWork unitOfWork,
                                                        ISqlConnectionFactory sqlConnectionFactory) : ICommandHandler<RefundPaymentsForEventCommand>
    {
        public async Task<Result> ExecuteAsync(RefundPaymentsForEventCommand request, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();
            var transaction = await connection.BeginTransactionAsync(cancellationToken);

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

                @event.PaymentsRefunded();

                var saveChanges = await unitOfWork.CommitAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return saveChanges
                    ? Result.Success()
                    : Result.Failure(PaymentErrors.FailToPersistRefundInformation);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(PaymentErrors.FailToPersistRefundInformation);
            }
        }
    }
}