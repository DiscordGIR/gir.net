using gir.net.Infra;
using NetCord.Rest;

namespace gir.net.Views;

public class PingView(GIRContext context) : BaseView, IView<string>
{
    public ComponentContainerProperties CreateFrom(string text)
    {
        var container = CreateContainer([
            new TextDisplayProperties(text)
        ]);
        
        return container;
    }

    public ComponentContainerProperties CreateFrom(string title, string model2)
    {
        throw new NotSupportedException();
    }
}