using Eventix.Modules.Ticketing.Domain.Payments.Errors;
using Eventix.Modules.Ticketing.Domain.Payments.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Payments.UseCases.Refund
{
    internal sealed class RefundPaymentHandler(IPaymentRepository paymentRepository,
                                               IUnitOfWork unitOfWork) : ICommandHandler<RefundPaymentCommand>
    {
        public async Task<Result> ExecuteAsync(RefundPaymentCommand request, CancellationToken cancellationToken = default)
        {
            var payment = await paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken);
            if (payment is null)
                return Result.Failure(PaymentErrors.NotFound(request.PaymentId));

            var paymentResult = payment.Refund(request.Amount);

            if (paymentResult.IsFailure
                && paymentResult.Error is not null)
                return Result.Failure(paymentResult.Error);

            var saveChanges = await unitOfWork.CommitAsync(cancellationToken);
            return saveChanges
                ? Result.Success()
                : Result.Failure(PaymentErrors.FailToPersistRefundInformation);
        }
    }
}