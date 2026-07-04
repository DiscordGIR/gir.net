using gir.net.Domain.Entities;
using gir.net.Domain.Interfaces.Repositories;
using gir.net.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace gir.net.Infrastructure.Repositories;

public class UserRepository(Db db) : IUserRepository
{
    public async Task<User?> GetUserAsync(ulong id) =>
        await db.Users.FindAsync(id);

    public async Task<User> GetOrCreateAsync(ulong id)
    {
        var user = await db.Users.FindAsync(id);
        if (user is not null)
            return user;

        user = new User { Id = id };
        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();
        return user;
    }

    public async Task AddUserAsync(User user)
    {
        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        db.Users.Update(user);
        await db.SaveChangesAsync();
    }

    public async Task<int> AddWarnPointsAsync(ulong id, int points)
    {
        var user = await GetOrCreateAsync(id);
        user.WarnPoints += points;
        await db.SaveChangesAsync();
        return user.WarnPoints;
    }

    public async Task SetWasWarnKickedAsync(ulong id, bool wasWarnKicked = true)
    {
        var user = await GetOrCreateAsync(id);
        user.WasWarnKicked = wasWarnKicked;
        await db.SaveChangesAsync();
    }
}
