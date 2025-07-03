using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.GetById
{
    internal sealed class GetCustomerByIdHandler(ICustomerRepository customerRepository) : IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdResponse>
    {
        public async Task<Result<GetCustomerByIdResponse>> ExecuteAsync(GetCustomerByIdQuery request, CancellationToken cancellationToken = default)
        {
            var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken).ConfigureAwait(false);
            if (customer is null)
                return Result.Failure<GetCustomerByIdResponse>(CustomerErrors.NotFound(request.CustomerId));

            return Result.Success(new GetCustomerByIdResponse(customer.Id, customer.Email.Address, customer.Name.FirstName, customer.Name.LastName));
        }
    }
}