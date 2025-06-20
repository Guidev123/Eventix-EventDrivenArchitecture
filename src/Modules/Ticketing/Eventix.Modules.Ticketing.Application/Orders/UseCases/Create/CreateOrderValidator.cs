using Eventix.Modules.Ticketing.Domain.Orders.Errors;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Orders.UseCases.Create
{
    public sealed class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(c => c.CustomerId).NotEmpty().WithMessage(OrderErrors.InvalidCustomerId.Description);
        }
    }
}