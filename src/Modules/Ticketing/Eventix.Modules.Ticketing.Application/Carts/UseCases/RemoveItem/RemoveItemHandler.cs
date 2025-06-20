using Eventix.Modules.Ticketing.Application.Carts.Services;
using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;
using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.Domain.Events.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.RemoveItem
{
    internal sealed class RemoveItemHandler(ICartService cartService,
                                            ICustomerRepository customerRepository,
                                            ITicketTypeRepository ticketTypeRepository) : ICommandHandler<RemoveItemCommand>
    {
        public async Task<Result> ExecuteAsync(RemoveItemCommand request, CancellationToken cancellationToken = default)
        {
            var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer is null)
                return Result.Failure(CustomerErrors.NotFound(request.CustomerId));

            var ticketType = await ticketTypeRepository.GetByIdAsync(request.TicketTypeId, cancellationToken);
            if (ticketType is null)
                return Result.Failure(TicketTypeErrors.NotFound(request.TicketTypeId));

            return await cartService.RemoveItemAsync(customer.Id, ticketType.Id, cancellationToken);
        }
    }
}