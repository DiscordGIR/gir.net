using NetCord;
using NetCord.Rest;

namespace gir.net.Infra;

public static class InteractionMessages
{
    public static InteractionMessageProperties FromContainer(
        ComponentContainerProperties container,
        MessageFlags flags = MessageFlags.IsComponentsV2,
        AllowedMentionsProperties? mentions = null,
        string? content = null)
    {
        var message = new InteractionMessageProperties()
            .WithComponents([container])
            .WithFlags(flags)
            .WithAllowedMentions(mentions ?? AllowedMentionsProperties.None);

        if (content is not null)
            message = message.WithContent(content);

        return message;
    }

    public static void Apply(MessageOptions target, InteractionMessageProperties source)
    {
        target.Content = source.Content;
        target.Components = source.Components;
        target.Flags = source.Flags;
        target.AllowedMentions = source.AllowedMentions;
    }
}
