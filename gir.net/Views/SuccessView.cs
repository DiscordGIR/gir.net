using NetCord.Rest;

namespace gir.net.Views;

public sealed class SuccessView : BaseView, IView<string>
{
    public ComponentContainerProperties CreateFrom(string message)
    {
        return CreateContainer(
            [
                new TextDisplayProperties(message),
            ],
            DiscordColor.Green
        );
    }

    public ComponentContainerProperties CreateFrom(string title, string message)
    {
        return CreateContainer(
            [
                new TextDisplayProperties($"### {title}"),
                new TextDisplayProperties(message),
            ],
            DiscordColor.Green
        );
    }
}
