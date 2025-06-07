using Eventix.Modules.Events.Domain.Categories.Entities;

namespace Eventix.Modules.Events.Domain.Categories.Interfaces
{
    public interface ICategoryRepository : IDisposable
    {
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Update(Category category);

        void Insert(Category category);
    }
}