using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.Update
{
    internal sealed class UpdateCustomerHandler(ICustomerRepository customerRepository) : ICommandHandler<UpdateCustomerCommand>
    {
        public async Task<Result> ExecuteAsync(UpdateCustomerCommand request, CancellationToken cancellationToken = default)
        {
            var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken).ConfigureAwait(false);
            if (customer is null)
                return Result.Failure(CustomerErrors.NotFound(request.CustomerId));

            UpdateCustomerName(customer, request);

            var saveChanges = await customerRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges ? Result.Success() : Result.Failure(CustomerErrors.FailToUpdate);
        }

        private void UpdateCustomerName(Customer user, UpdateCustomerCommand command)
        {
            user.UpdateName(command.FirstName!, command.LastName!);
            customerRepository.Update(user);
        }
    }
}