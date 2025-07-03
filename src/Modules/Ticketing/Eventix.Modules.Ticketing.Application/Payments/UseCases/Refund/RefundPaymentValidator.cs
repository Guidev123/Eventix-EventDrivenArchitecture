using Eventix.Modules.Ticketing.Domain.Payments.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Payments.UseCases.Refund
{
    internal sealed class RefundPaymentValidator : AbstractValidator<RefundPaymentCommand>
    {
        public RefundPaymentValidator()
        {
            RuleFor(r => r.PaymentId).NotEmpty().WithMessage(PaymentErrors.InvalidPaymentData.Description);
        }
    }
}