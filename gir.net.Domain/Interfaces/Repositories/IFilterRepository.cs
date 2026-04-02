using gir.net.Domain.Entities;

namespace gir.net.Domain.Interfaces.Repositories;

public interface IFilterRepository
{
    Task AddFilterWordAsync(FilterWord filterWord);
    Task<IEnumerable<FilterWord>> GetFilteredWordsPaginatedAsync(int page, int take);
    Task<int> CountFilterWordsAsync();
}