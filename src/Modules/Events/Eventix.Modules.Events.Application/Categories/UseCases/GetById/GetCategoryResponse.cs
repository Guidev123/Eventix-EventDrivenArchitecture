namespace Eventix.Modules.Events.Application.Categories.UseCases.GetById
{
    public record GetCategoryResponse(Guid Id, string Name, bool IsArchived);
}