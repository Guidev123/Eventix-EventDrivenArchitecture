using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Categories.UseCases.Create
{
    public record CreateCategoryCommand(string Name) : ICommand<CreateCategoryResponse>;
}