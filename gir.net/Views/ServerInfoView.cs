using gir.net.Infra;
using NetCord;
using NetCord.Rest;

namespace gir.net.Views;

public class ServerInfoView(GIRContext context) : BaseView, IView<string>
{
    public ComponentContainerProperties CreateFrom(string model)
    {
        var guild = context.Guild!;
        var thumbnail = new ComponentSectionThumbnailProperties(guild.GetIconUrl()!.ToString());
        var container = CreateContainer([
                new TextDisplayProperties("### Server information"),
                new ComponentSectionProperties(thumbnail)
                    .WithComponents(
                        [
                            new TextDisplayProperties($"**Users**\n{guild.UserCount}"),
                            new TextDisplayProperties($"**Channels**\n{guild.Channels.Count}"),
                            new TextDisplayProperties($"**Roles**\n{guild.Roles.Count}"),
                        ]
                    ),
                new TextDisplayProperties($"**Bans**\nTODO"),
                new TextDisplayProperties($"**Emojis**\n{guild.Emojis.Count}"),
                new TextDisplayProperties($"**Boost tier**\n{guild.PremiumTier}"),
                new TextDisplayProperties($"**Owner**\n{guild.Users[guild.OwnerId].ToString()}"),
                new TextDisplayProperties(
                    $"**Created**\n{new Timestamp(guild.CreatedAt, TimestampStyle.LongDateTime)}"),
            ]
        );

        if (guild.HasBanner)
        {
            container.Add(new MediaGalleryProperties().WithItems([
                    new MediaGalleryItemProperties(new ComponentMediaProperties(guild.GetBannerUrl()?.ToString()))
                ])
            );
        }

        return container;
    }

    public ComponentContainerProperties CreateFrom(string title, string model2)
    {
        throw new NotSupportedException();
    }
}