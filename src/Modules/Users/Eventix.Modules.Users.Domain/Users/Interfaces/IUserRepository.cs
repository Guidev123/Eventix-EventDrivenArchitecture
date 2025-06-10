using Eventix.Modules.Users.Domain.Users.Entities;
using Eventix.Shared.Domain.Interfaces;

namespace Eventix.Modules.Users.Domain.Users.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        void Insert(User user);

        void Update(User user);

        Task<User?> GetByIdAsync(Guid id);
    }
}