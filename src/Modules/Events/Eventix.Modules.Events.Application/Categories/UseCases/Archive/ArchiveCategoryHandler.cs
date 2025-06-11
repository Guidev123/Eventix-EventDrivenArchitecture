using Eventix.Modules.Events.Domain.Categories.Errors;
using Eventix.Modules.Events.Domain.Categories.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.Categories.UseCases.Archive
{
    internal sealed class ArchiveCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : ICommandHandler<ArchiveCategoryCommand>
    {
        public async Task<Result> ExecuteAsync(ArchiveCategoryCommand request, CancellationToken cancellationToken = default)
        {
            var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            if (category is null)
                return Result.Failure(CategoryErrors.NotFound(request.CategoryId));

            if (category.IsArchived)
                return Result.Failure(CategoryErrors.AlreadyArchived);

            category.Archive();
            categoryRepository.Update(category);

            var saveChanges = await unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges ? Result.Success() : Result.Failure(CategoryErrors.FailToArchive);
        }
    }
}