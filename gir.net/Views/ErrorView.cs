using NetCord;
using NetCord.Rest;
using Color = System.Drawing.Color;

namespace gir.net.Views;

public sealed class ErrorView : BaseView, IView<string>
{
    public ComponentContainerProperties CreateFrom(string message)
    {
        return CreateContainer(
            [
                new TextDisplayProperties("### :("),
                new TextDisplayProperties("### Your command ran into a problem"),
                new TextDisplayProperties(message),
            ],
            Color.OrangeRed
        );
    }
}