using gir.net.Domain.Entities;
using gir.net.Domain.Interfaces.Repositories;
using gir.net.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace gir.net.Infrastructure.Repositories;

public class CaseRepository(Db db) : ICaseRepository
{
    public async Task<Case?> GetByIdAsync(int id)
    {
        return await db.Cases.FindAsync(id);
    }

    public async Task AddCaseAsync(Case entity)
    {
        await db.Cases.AddAsync(entity);
        await db.SaveChangesAsync();
    }

    public async Task UpdateCaseAsync(Case entity)
    {
        db.Cases.Update(entity);
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Case>> GetCasesByUserIdAsync(ulong userId, int take = 50)
    {
        return await db.Cases
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.Date)
            .Take(take)
            .ToListAsync();
    }
}
