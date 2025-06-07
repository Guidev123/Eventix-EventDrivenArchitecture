using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Categories.Archive
{
    public record ArchiveCategoryCommand(Guid CategoryId) : ICommand;
}