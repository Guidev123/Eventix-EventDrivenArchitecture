using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Categories.UseCases.GetById
{
    public record GetCategoryByIdQuery(Guid CategoryId) : IQuery<GetCategoryResponse>;
}