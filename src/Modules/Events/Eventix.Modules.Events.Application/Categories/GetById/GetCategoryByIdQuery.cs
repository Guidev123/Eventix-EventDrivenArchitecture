using Eventix.Modules.Events.Application.Abstractions.Messaging;

namespace Eventix.Modules.Events.Application.Categories.GetById
{
    public record GetCategoryByIdQuery(Guid CategoryId) : IQuery<GetCategoryResponse>;
}