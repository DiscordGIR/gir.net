using gir.net.Domain.Entities;
using gir.net.Domain.Interfaces.Repositories;
using gir.net.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace gir.net.Infrastructure.Repositories;

public class UserRepository(Db db) : IUserRepository
{
    public async Task<User?> GetUserAsync(long id)
    {
        return await db.Users.FindAsync(id);
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
}
