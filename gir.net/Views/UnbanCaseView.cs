using gir.net.Domain.Entities;
using NetCord;
using NetCord.Rest;
namespace gir.net.Views;

public sealed class UnbanCaseView : BaseView
{
    public ComponentContainerProperties CreateFrom(Case unbanCase, NetCord.User target, GuildUser mod)
    {
        var avatarUrl = target.GetAvatarUrl()?.ToString() ?? target.DefaultAvatarUrl.ToString();
        var thumbnail = new ComponentSectionThumbnailProperties(avatarUrl);

        return CreateContainer(
            [
                new TextDisplayProperties("### Member Unbanned"),
                new ComponentSectionProperties(thumbnail)
                    .WithComponents(
                        [
                            new TextDisplayProperties($"**Member**\n{target.Username} ({target.Id})"),
                            new TextDisplayProperties($"**Mod**\n{mod.Username} ({mod})"),
                        ]
                    ),
                new TextDisplayProperties($"**Reason**\n{unbanCase.Reason}"),
                new TextDisplayProperties(
                    $"-# Case #{unbanCase.Id} | {target.Id} · {new Timestamp(unbanCase.Date, TimestampStyle.LongDateTime)}"),
            ],
            DiscordColor.Blurple
        );
    }
}
