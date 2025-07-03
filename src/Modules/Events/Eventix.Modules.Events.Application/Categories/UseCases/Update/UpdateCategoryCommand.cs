using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Categories.UseCases.Update
{
    public sealed record UpdateCategoryCommand : ICommand
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