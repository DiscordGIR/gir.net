using gir.net.Domain.Entities;
using NetCord;
using NetCord.Rest;
namespace gir.net.Views;

public sealed class BanCaseView : BaseView
{
    public ComponentContainerProperties CreateFrom(Case banCase, NetCord.User target, GuildUser mod)
    {
        var avatarUrl = target.GetAvatarUrl()?.ToString() ?? target.DefaultAvatarUrl.ToString();
        var thumbnail = new ComponentSectionThumbnailProperties(avatarUrl);

        return CreateContainer(
            [
                new TextDisplayProperties("### Member Banned"),
                new ComponentSectionProperties(thumbnail)
                    .WithComponents(
                        [
                            new TextDisplayProperties($"**Member**\n{target.Username} ({target})"),
                            new TextDisplayProperties($"**Mod**\n{mod.Username} ({mod})"),
                        ]
                    ),
                new TextDisplayProperties($"**Reason**\n{banCase.Reason}"),
                new TextDisplayProperties(
                    $"-# Case #{banCase.Id} | {target.Id} · {new Timestamp(banCase.Date, TimestampStyle.LongDateTime)}"),
            ],
            DiscordColor.Blue
        );
    }
}
