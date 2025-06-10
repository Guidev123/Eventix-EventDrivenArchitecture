using Eventix.Modules.Events.Domain.Categories.Entities;
using Eventix.Modules.Events.Domain.Categories.Errors;
using Eventix.Modules.Events.Domain.Categories.Interfaces;
using Eventix.Modules.Events.Domain.Shared.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.Categories.UseCases.Create
{
    internal sealed class CreateCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateCategoryCommand, CreateCategoryResponse>
    {
        public async Task<Result<CreateCategoryResponse>> ExecuteAsync(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = Category.Create(request.Name);

            categoryRepository.Insert(category);

            var saveChanges = await PersistDataAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges ? Result.Success(new CreateCategoryResponse(category.Id))
                : Result.Failure<CreateCategoryResponse>(CategoryErrors.FailToCreate);
        }

        private async ValueTask<bool> PersistDataAsync(CancellationToken cancellationToken)
            => await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }
}