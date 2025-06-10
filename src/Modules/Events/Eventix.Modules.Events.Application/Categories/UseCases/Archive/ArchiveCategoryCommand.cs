using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Categories.UseCases.Archive
{
    public record ArchiveCategoryCommand(Guid CategoryId) : ICommand;
}