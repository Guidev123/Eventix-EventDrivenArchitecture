using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Categories.UseCases.GetAll
{
    public sealed record GetAllCategoriesQuery(int Page, int PageSize) : IQuery<GetAllCategoriesResponse>;
}