using Eventix.Modules.Events.Application.Abstractions.Messaging;

namespace Eventix.Modules.Events.Application.Categories.Update
{
    public record UpdateCategoryCommand : ICommand
    {
        public UpdateCategoryCommand(string name)
        {
            Name = name;
        }

        public Guid? CategoryId { get; private set; }
        public string Name { get; } = string.Empty;
        public void SetCategoryId(Guid id) => CategoryId = id;
    }
}