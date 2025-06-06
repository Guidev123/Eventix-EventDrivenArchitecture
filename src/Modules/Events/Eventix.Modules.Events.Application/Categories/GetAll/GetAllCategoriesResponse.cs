namespace Eventix.Modules.Events.Application.Categories.GetAll
{
    public record GetAllCategoriesResponse(Guid Id, string Name, bool IsArchived);
}