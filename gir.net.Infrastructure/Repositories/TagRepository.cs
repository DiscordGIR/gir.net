using gir.net.Domain.Entities;
using gir.net.Domain.Exceptions;
using gir.net.Domain.Interfaces.Repositories;
using gir.net.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace gir.net.Infrastructure.Repositories;

public class TagRepository(Db db) : ITagRepository
{
    public async Task<Tag?> GetTagAsync(string name)
    {
        return await db.Tags.FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task AddTagAsync(Tag tag)
    {
        try
        {
            await db.Tags.AddAsync(tag);
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (IsPostgresUniqueViolation(ex))
        {
            throw new DuplicateTagNameException(tag.Name, ex);
        }
    }

    private static bool IsPostgresUniqueViolation(DbUpdateException ex)
    {
        for (var inner = ex.InnerException; inner != null; inner = inner.InnerException)
        {
            if (inner is PostgresException pg && pg.SqlState == PostgresErrorCodes.UniqueViolation)
                return true;
        }

        return false;
    }

    public async Task UpdateTagAsync(Tag tag)
    {
        db.Tags.Update(tag);
        await db.SaveChangesAsync();
    }

    public async Task DeleteTagAsync(string name)
    {
        var tag = await GetTagAsync(name);
        if (tag != null)
        {
            db.Tags.Remove(tag);
            await db.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Tag>> SearchTagsAsync(string searchText)
    {
        var tags = await db.Tags
            .Where(t => string.IsNullOrEmpty(searchText) || t.Name.Contains(searchText))
            .Take(25)
            .ToListAsync();

        return tags;
    }
}
