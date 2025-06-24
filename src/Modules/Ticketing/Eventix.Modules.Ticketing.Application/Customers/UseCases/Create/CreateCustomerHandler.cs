using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.Create
{
    public sealed class CreateCustomerHandler(ICustomerRepository customerRepository) : ICommandHandler<CreateCustomerCommand, CreateCustomerResponse>
    {
        public async Task<Result<CreateCustomerResponse>> ExecuteAsync(CreateCustomerCommand request, CancellationToken cancellationToken = default)
        {
            var customerAlreadyExists = await customerRepository.ExistsAsync(request.CustomerId, cancellationToken).ConfigureAwait(false);
            if (customerAlreadyExists)
                return Result.Failure<CreateCustomerResponse>(CustomerErrors.AlreadyExists);

            var customer = Customer.Create(request.CustomerId, request.Email, request.FirstName, request.LastName);

            customerRepository.Insert(customer);

            var saveChanges = await customerRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges
                ? Result.Success(new CreateCustomerResponse(customer.Id))
                : Result.Failure<CreateCustomerResponse>(CustomerErrors.SomethingHasFailedDuringPersistence);
        }
    }
}