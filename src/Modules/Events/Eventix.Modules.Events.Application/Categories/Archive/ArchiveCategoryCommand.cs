using Eventix.Modules.Events.Application.Abstractions.Messaging;

namespace Eventix.Modules.Events.Application.Categories.Archive
{
    public record ArchiveCategoryCommand(Guid CategoryId) : ICommand;
}