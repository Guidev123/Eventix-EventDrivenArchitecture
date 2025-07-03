using Eventix.Modules.Events.Application.Categories.UseCases.GetById;

namespace Eventix.Modules.Events.Application.Categories.UseCases.GetAll
{
    public sealed record GetAllCategoriesResponse(
        int Page,
        int PageSize,
        int TotalCount,
        IReadOnlyCollection<GetCategoryResponse> Categories);
}