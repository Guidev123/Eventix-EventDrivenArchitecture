using Eventix.Modules.Events.Domain.Categories.Entities;
using Eventix.Modules.Events.Domain.Categories.Interfaces;
using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Events.Infrastructure.Categories.Repositories
{
    internal sealed class CategoryRepository(EventsDbContext context) : ICategoryRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Categories.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public void Insert(Category category)
            => context.Categories.Add(category);

        public void Update(Category category)
            => context.Categories.Update(category);

        public void Dispose()
            => context.Dispose();
    }
}