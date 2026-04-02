using NetCord.Rest;
using Color = System.Drawing.Color;

namespace gir.net.Views;

public abstract class BaseView
{
    protected static ComponentContainerProperties CreateContainer(
        IEnumerable<IComponentContainerComponentProperties> components,
        Color? accentColor = null)
    {
        var container = new ComponentContainerProperties()
            .WithComponents(components);

        if (accentColor.HasValue)
        {
            container = container.WithAccentColor(new NetCord.Color(accentColor.Value.ToArgb()));
        }

        return container;
    }
}
