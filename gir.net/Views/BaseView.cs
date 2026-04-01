using NetCord.Rest;
using Color = System.Drawing.Color;

namespace gir.net.Views;

public abstract class BaseView
{
    protected static ComponentContainerProperties CreateContainer(
        IEnumerable<IComponentContainerComponentProperties> components,
        Color accentColor)
    {
        return new ComponentContainerProperties()
            .WithComponents(components)
            .WithAccentColor(new(accentColor.ToArgb()));
    }
}
