using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Events.Domain.Categories.DomainEvents
{
    public record CategoryCreatedDomainEvent : DomainEvent
    {
        public CategoryCreatedDomainEvent(Guid categoryId, string name) : base(categoryId)
        {
            CategoryId = categoryId;
            Name = name;
        }

        public Guid CategoryId { get; }
        public string Name { get; }
    }
}