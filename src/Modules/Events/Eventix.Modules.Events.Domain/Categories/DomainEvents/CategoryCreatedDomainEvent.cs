using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Domain.Categories.DomainEvents
{
    public record CategoryCreatedDomainEvent : DomainEvent
    {
        public CategoryCreatedDomainEvent(Guid categoryId, string name)
        {
            CategoryId = categoryId;
            Name = name;
        }

        public Guid CategoryId { get; }
        public string Name { get; }
    }
}