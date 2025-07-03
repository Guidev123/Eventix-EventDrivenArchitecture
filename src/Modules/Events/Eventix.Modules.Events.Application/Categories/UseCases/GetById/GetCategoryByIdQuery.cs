using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Categories.UseCases.GetById
{
    public sealed record GetCategoryByIdQuery(Guid CategoryId) : IQuery<GetCategoryResponse>;
}