using Eventix.Modules.Ticketing.Domain.Payments.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Payments.UseCases.RefundForEvent
{
    internal sealed class RefundPaymentsForEventValidator : AbstractValidator<RefundPaymentsForEventCommand>
    {
        public RefundPaymentsForEventValidator()
        {
            RuleFor(r => r.EventId).NotEmpty().WithMessage(PaymentErrors.InvalidEventId.Description);
        }
    }
}