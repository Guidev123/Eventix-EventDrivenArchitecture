using Eventix.Modules.Events.Application.Abstractions.Messaging;

namespace Eventix.Modules.Events.Application.Categories.Create
{
    public record CreateCategoryCommand(string Name) : ICommand<CreateCategoryResponse>;
}