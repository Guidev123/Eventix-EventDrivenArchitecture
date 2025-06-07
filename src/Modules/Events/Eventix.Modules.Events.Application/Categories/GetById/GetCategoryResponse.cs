namespace Eventix.Modules.Events.Application.Categories.GetById
{
    public record GetCategoryResponse(Guid Id, string Name, bool IsArchived);
}