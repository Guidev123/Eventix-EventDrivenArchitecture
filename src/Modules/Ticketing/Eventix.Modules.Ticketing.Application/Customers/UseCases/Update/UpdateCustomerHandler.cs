using Eventix.Modules.Ticketing.Domain.Customers.Entities;
using Eventix.Modules.Ticketing.Domain.Customers.Errors;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.Update
{
    public sealed class UpdateCustomerHandler(ICustomerRepository customerRepository,
                                              IUnitOfWork unitOfWork) : ICommandHandler<UpdateCustomerCommand>
    {
        public async Task<Result> ExecuteAsync(UpdateCustomerCommand request, CancellationToken cancellationToken = default)
        {
            var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken).ConfigureAwait(false);
            if (customer is null)
                return Result.Failure(CustomerErrors.NotFound(request.CustomerId));

            UpdateProperties(customer, request);

            var saveChanges = await unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges ? Result.Success() : Result.Failure(CustomerErrors.FailToUpdate);
        }

        private void UpdateProperties(Customer customer, UpdateCustomerCommand command)
        {
            if (IsNameChange(command))
                UpdateCustomerName(customer, command);

            if (IsEmailChange(command))
                UpdateCustomerEmail(customer, command);
        }

        private void UpdateCustomerName(Customer user, UpdateCustomerCommand command)
        {
            if (!IsNameChange(command)) return;

            user.UpdateName(command.FirstName!, command.LastName!);
            customerRepository.Update(user);
        }

        private void UpdateCustomerEmail(Customer user, UpdateCustomerCommand command)
        {
            if (!IsEmailChange(command)) return;

            user.UpdateEmail(command.Email!);
            customerRepository.Update(user);
        }

        private static bool IsNameChange(UpdateCustomerCommand command)
            => !string.IsNullOrEmpty(command.FirstName) || !string.IsNullOrEmpty(command.LastName);

        private static bool IsEmailChange(UpdateCustomerCommand command)
            => command.Email is not null && !string.IsNullOrEmpty(command.Email.Trim());
    }
}