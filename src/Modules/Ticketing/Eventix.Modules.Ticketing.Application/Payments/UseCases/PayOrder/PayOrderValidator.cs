using Eventix.Modules.Ticketing.Domain.Payments.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Payments.UseCases.PayOrder
{
    internal sealed class PayOrderValidator : AbstractValidator<PayOrderCommand>
    {
        public PayOrderValidator()
        {
            RuleFor(c => c.OrderId)
                .NotEmpty()
                .WithMessage(PaymentErrors.InvalidOrderId.Description);

            RuleFor(c => c.TotalPrice)
                .Must(decimal.IsPositive)
                .WithMessage(PaymentErrors.InvalidPaymentData.Description);

            RuleFor(c => c.Currency)
                .NotEmpty()
                .WithMessage(PaymentErrors.InvalidPaymentData.Description);
        }
    }
}