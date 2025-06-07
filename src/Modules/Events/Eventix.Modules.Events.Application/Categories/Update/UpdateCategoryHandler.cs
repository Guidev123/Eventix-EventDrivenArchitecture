using Eventix.Modules.Events.Application.Abstractions.Data;
using Eventix.Modules.Events.Application.Abstractions.Messaging;
using Eventix.Modules.Events.Domain.Categories.Errors;
using Eventix.Modules.Events.Domain.Categories.Interfaces;
using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Application.Categories.Update
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

            var rows = await unitOfWork.SaveChangesAsync(cancellationToken);

            return rows > 0
                ? Result.Success()
                : Result.Failure(Error.Problem("Categories.Update", "An error occurred while updating the category."));
        }
    }
}