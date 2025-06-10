using Eventix.Modules.Events.Domain.Categories.Errors;
using Eventix.Modules.Events.Domain.Categories.Interfaces;
using Eventix.Modules.Events.Domain.Shared.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.Categories.UseCases.Update
{
    internal sealed class UpdateCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateCategoryCommand>
    {
        public async Task<Result> ExecuteAsync(UpdateCategoryCommand request, CancellationToken cancellationToken = default)
        {
            var category = await categoryRepository.GetByIdAsync(request.CategoryId!.Value, cancellationToken);
            if (category is null)
                return Result.Failure(CategoryErrors.NotFound(request.CategoryId.Value));

            category.Rename(request.Name);
            categoryRepository.Update(category);

            var saveChanges = await PersistDataAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges
                ? Result.Success()
                : Result.Failure(CategoryErrors.FailToUpdate);
        }

        private async ValueTask<bool> PersistDataAsync(CancellationToken cancellationToken)
            => await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }
}