using NetCord.Rest;
using Color = System.Drawing.Color;

namespace gir.net.Views;

public sealed class SuccessView : BaseView, IView<string>
{
    public ComponentContainerProperties CreateFrom(string message)
    {
        return CreateContainer(
            [
                new TextDisplayProperties(message),
            ],
            Color.Green
        );
    }
}
