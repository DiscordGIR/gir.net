using gir.net.Domain.Entities;

namespace gir.net.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserAsync(long id);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
}
