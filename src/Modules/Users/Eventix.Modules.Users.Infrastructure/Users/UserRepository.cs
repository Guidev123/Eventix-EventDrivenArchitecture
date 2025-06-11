using Eventix.Modules.Users.Domain.Users.Entities;
using Eventix.Modules.Users.Domain.Users.Interfaces;
using Eventix.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Users.Infrastructure.Users
{
    internal sealed class UserRepository(UsersDbContext context) : IUserRepository
    {
        public async Task<User?> GetByIdAsync(Guid id)
            => await context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);

        public void Insert(User user) => context.Add(user);

        public void Update(User user) => context.Update(user);

        public void Dispose() => context.Dispose();
    }
}