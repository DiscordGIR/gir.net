using gir.net.Application.Interfaces.Services;
using gir.net.Views;
using NetCord.Rest;

namespace gir.net.Modules.Filter;

public class FilterListPageBuilder(IFilterService filterService)
{
    public async Task<ComponentContainerProperties> BuildAsync(int page)
    {
        var take = FilterListPagination.PageSize;
        var totalCount = await filterService.CountFilterWordsAsync();
        var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)take);

        if (totalPages > 0)
            page = Math.Clamp(page, 0, totalPages - 1);
        else
            page = 0;

        var items = await filterService.GetFilterWordsPaginatedAsync(page, take);
        return new FilterListView().CreateFrom("filter", page, totalPages, items);
    }
}
