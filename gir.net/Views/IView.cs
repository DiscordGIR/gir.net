using NetCord.Rest;

namespace gir.net.Views;

public interface IView<in TModel>
{
    ComponentContainerProperties CreateFrom(TModel model);
    ComponentContainerProperties CreateFrom(string title, TModel model2);
}
