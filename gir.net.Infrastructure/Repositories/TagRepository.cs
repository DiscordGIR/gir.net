using gir.net.Domain.Entities;
using gir.net.Domain.Interfaces.Repositories;
using gir.net.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace gir.net.Infrastructure.Repositories;

public class TagRepository(Db db) : ITagRepository
{
    public async Task<Tag?> GetTagAsync(string name)
    {
        return await db.Tags.FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task AddTagAsync(Tag tag)
    {
        await db.Tags.AddAsync(tag);
        await db.SaveChangesAsync();
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
