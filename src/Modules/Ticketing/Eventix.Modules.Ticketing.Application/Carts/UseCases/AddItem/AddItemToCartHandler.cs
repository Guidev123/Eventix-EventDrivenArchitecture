using Eventix.Modules.Ticketing.Application.Carts.Models;
using Eventix.Modules.Ticketing.Application.Carts.Services;
using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;
using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.Domain.Events.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.AddItem
{
    internal sealed class AddItemToCartHandler(ICartService cartService,
                                               ICustomerRepository customerRepository,
                                               ITicketTypeRepository ticketTypeRepository) : ICommandHandler<AddItemToCartCommand>
    {
        public async Task<Result> ExecuteAsync(AddItemToCartCommand request, CancellationToken cancellationToken = default)
        {
            var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken).ConfigureAwait(false);
            if (customer is null)
                return Result.Failure(CustomerErrors.NotFound(request.CustomerId));

            var ticketType = await ticketTypeRepository.GetByIdAsync(request.TicketTypeId, cancellationToken).ConfigureAwait(false);
            if (ticketType is null)
                return Result.Failure(TicketTypeErrors.NotFound(request.TicketTypeId));

            if (ticketType.Specification.Quantity < request.Quantity)
                return Result.Failure(TicketTypeErrors.NotEnoughQuantity(ticketType.Specification.Quantity));

            var cartItem = new CartItem()
            {
                TicketTypeId = ticketType.Id,
                Quantity = request.Quantity,
                Amount = ticketType.Price.Amount,
                Currency = ticketType.Price.Currency,
            };

            return await cartService.AddItemAsync(customer.Id, cartItem, cancellationToken).ConfigureAwait(false);
        }
    }
}