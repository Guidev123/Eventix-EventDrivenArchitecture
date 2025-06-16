using Eventix.Modules.Ticketing.Application.Carts.Services;
using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.Clear
{
    public sealed class ClearCartHandler(ICustomerRepository customerRepository,
                                         ICartService cartService) : ICommandHandler<ClearCartCommand>
    {
        public async Task<Result> ExecuteAsync(ClearCartCommand request, CancellationToken cancellationToken = default)
        {
            var customer = await customerRepository.ExistsAsync(request.CustomerId, cancellationToken).ConfigureAwait(false);
            if (!customer)
                return Result.Failure(CustomerErrors.NotFound(request.CustomerId));

            return await cartService.ClearAsync(request.CustomerId, cancellationToken).ConfigureAwait(false);
        }
    }
}