using gir.net.Domain.Entities;

namespace gir.net.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserAsync(ulong id);
    Task<User> GetOrCreateAsync(ulong id);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task<int> AddWarnPointsAsync(ulong id, int points);
    Task SetWasWarnKickedAsync(ulong id, bool wasWarnKicked = true);
}
