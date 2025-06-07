using Eventix.Modules.Events.Application.Abstractions.Messaging;

namespace Eventix.Modules.Events.Application.Categories.GetAll
{
    public record GetAllCategoriesQuery(int Page, int PageSize) : IQuery<GetAllCategoriesResponse>;
}