using Eventix.Modules.Events.Application.Abstractions.Data;
using Eventix.Modules.Events.Application.Abstractions.Messaging;
using Eventix.Modules.Events.Domain.Categories.Entities;
using Eventix.Modules.Events.Domain.Categories.Errors;
using Eventix.Modules.Events.Domain.Categories.Interfaces;
using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Application.Categories.Create
{
    internal sealed class CreateCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateCategoryCommand, CreateCategoryResponse>
    {
        public async Task<Result<CreateCategoryResponse>> ExecuteAsync(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = Category.Create(request.Name);

            categoryRepository.Insert(category);

            var rows = await unitOfWork.SaveChangesAsync(cancellationToken);

            return rows > 0
                ? Result.Success(new CreateCategoryResponse(category.Id))
                : Result.Failure<CreateCategoryResponse>(CategoryErrors.FailToCreate);
        }
    }
}