using Eventix.Modules.Events.Domain.Categories.Entities;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Events.Domain.Categories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Update(Category category);

        void Insert(Category category);
    }
}