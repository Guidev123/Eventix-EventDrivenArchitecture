using Eventix.Modules.Events.Application.Categories.GetById;

namespace Eventix.Modules.Events.Application.Categories.GetAll
{
    public record GetAllCategoriesResponse(
        int Page,
        int PageSize,
        int TotalCount,
        IReadOnlyCollection<GetCategoryResponse> Categories);
}