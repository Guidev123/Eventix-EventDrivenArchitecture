using Eventix.Modules.Events.Domain.Categories.Errors;
using Eventix.Modules.Events.Domain.Categories.Interfaces;
using Eventix.Modules.Events.Domain.Shared.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.Categories.Archive
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

            var rows = await unitOfWork.SaveChangesAsync(cancellationToken);
            return rows > 0
                ? Result.Success()
                : Result.Failure(CategoryErrors.FailToArchive);
        }
    }
}