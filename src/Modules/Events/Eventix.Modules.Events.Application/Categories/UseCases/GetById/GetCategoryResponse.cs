namespace Eventix.Modules.Events.Application.Categories.UseCases.GetById
{
    public sealed record GetCategoryResponse(Guid Id, string Name, bool IsArchived);
}