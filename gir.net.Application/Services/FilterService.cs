using gir.net.Application.Interfaces.Services;
using gir.net.Domain.Entities;
using gir.net.Domain.Interfaces.Repositories;

namespace gir.net.Application.Services;

public class FilterService(IFilterRepository filterRepository) : IFilterService
{
    public async Task AddFilterWordAsync(FilterWord filterWord)
    {
        await filterRepository.AddFilterWordAsync(filterWord);
    }

    public async Task<IEnumerable<FilterWord>> GetFilterWordsPaginatedAsync(int page, int take)
    {
        return await filterRepository.GetFilteredWordsPaginatedAsync(page, take);
    }

    public Task<int> CountFilterWordsAsync() => filterRepository.CountFilterWordsAsync();
}