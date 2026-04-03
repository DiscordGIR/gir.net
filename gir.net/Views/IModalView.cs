using NetCord.Rest;

namespace gir.net.Views;

public interface IModalView<in T>
{
    public ModalProperties CreateFrom(T  tagName);
}