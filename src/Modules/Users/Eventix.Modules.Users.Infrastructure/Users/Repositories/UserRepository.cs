using Eventix.Modules.Users.Domain.Users.Entities;
using Eventix.Modules.Users.Domain.Users.Interfaces;
using Eventix.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Users.Infrastructure.Users.Repositories
{
    internal sealed class UserRepository(UsersDbContext context) : IUserRepository
    {
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

        public void Insert(User user) => context.Users.Add(user);

        public void Update(User user) => context.Users.Update(user);

        public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
            => await context.Users.AnyAsync(user => user.Email.Address == email, cancellationToken);

        public void Dispose() => context.Dispose();
    }
}