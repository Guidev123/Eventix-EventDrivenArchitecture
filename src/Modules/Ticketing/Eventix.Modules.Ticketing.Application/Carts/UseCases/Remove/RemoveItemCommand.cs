using Eventix.Modules.Ticketing.Application.Carts.Errors;
using Eventix.Modules.Ticketing.Application.Carts.Services;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;
using FluentValidation;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.Remove
{
    public record RemoveItemCommand : ICommand
    {
        public RemoveItemCommand(Guid ticketTypeId)
        {
            TicketTypeId = ticketTypeId;
        }

        public Guid CustomerId { get; private set; }
        public Guid TicketTypeId { get; }
        public void SetCustomerId(Guid customerId) => CustomerId = customerId;
    }

    public sealed class RemoveItemValidator : AbstractValidator<RemoveItemCommand>
    {
        public RemoveItemValidator()
        {
            RuleFor(i => i.CustomerId).NotEmpty().WithMessage(CartErrors.CustomerIdIsRequired.Description);
            RuleFor(i => i.TicketTypeId).NotEmpty().WithMessage(CartErrors.TicketTypeIdIsRequired.Description);
        }
    }

    internal sealed class RemoveItemHandler(ICartService cartService,
                                            ICustomerRepository customerRepository,
                                            ITicketTypeRepository ticketTypeRepository) : ICommandHandler<RemoveItemCommand>
    {
        public async Task<Result> ExecuteAsync(RemoveItemCommand request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}