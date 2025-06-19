using Eventix.Modules.Events.PublicApi;
using Eventix.Modules.Ticketing.Application.Carts.Models;
using Eventix.Modules.Ticketing.Application.Carts.Services;
using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;
using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.AddItem
{
    internal sealed class AddItemToCartHandler(ICartService cartService,
                                               ICustomerRepository customerRepository,
                                               IEventsApi eventsApi) : ICommandHandler<AddItemToCartCommand>
    {
        public async Task<Result> ExecuteAsync(AddItemToCartCommand request, CancellationToken cancellationToken = default)
        {
            var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken).ConfigureAwait(false);
            if (customer is null)
                return Result.Failure(CustomerErrors.NotFound(request.CustomerId));

            var ticketType = await eventsApi.GetTicketTypeAsync(request.TicketTypeId, cancellationToken).ConfigureAwait(false);
            if (ticketType is null)
                return Result.Failure(TicketTypeErrors.NotFound(request.TicketTypeId));

            var cartItem = new CartItem()
            {
                TicketTypeId = ticketType.Id,
                Quantity = request.Quantity,
                Amount = ticketType.Amount,
                Currency = ticketType.Currency,
            };

            return await cartService.AddItemAsync(customer.Id, cartItem, cancellationToken).ConfigureAwait(false);
        }
    }
}