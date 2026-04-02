using NetCord.Rest;

namespace gir.net.Views;

public interface IPaginatedView<in TModel>
{
    ComponentContainerProperties CreateFrom(string id, int currentPage, int totalPageCount, IEnumerable<TModel> models);
}