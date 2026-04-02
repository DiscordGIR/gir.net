using gir.net.Domain.Entities;
using gir.net.Domain.Exceptions;
using gir.net.Domain.Interfaces.Repositories;
using gir.net.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace gir.net.Infrastructure.Repositories;

public class FilterRepository(Db db) : IFilterRepository
{
    public async Task AddFilterWordAsync(FilterWord filterWord)
    {
        try
        {
            await db.FilterWords.AddAsync(filterWord);
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (DbHelpers.IsPostgresUniqueViolation(ex))
        {
            throw new DuplicateFilerWordException(filterWord.Phrase, ex);
        }
    }

    public async Task<IEnumerable<FilterWord>> GetFilteredWordsPaginatedAsync(int page, int take)
    {
        return await db.FilterWords.OrderBy(fw => fw.Phrase).Skip(page * take).Take(take).ToListAsync();
    }

    public Task<int> CountFilterWordsAsync() => db.FilterWords.CountAsync();
}