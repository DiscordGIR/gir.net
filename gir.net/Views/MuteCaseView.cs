using gir.net.Domain.Entities;
using NetCord;
using NetCord.Rest;

namespace gir.net.Views;

public sealed class MuteCaseView : BaseView
{
    public ComponentContainerProperties CreateFrom(Case muteCase, GuildUser target, GuildUser mod)
    {
        var avatarUrl = target.GetAvatarUrl()?.ToString() ?? target.DefaultAvatarUrl.ToString();
        var thumbnail = new ComponentSectionThumbnailProperties(avatarUrl);

        return CreateContainer(
            [
                new TextDisplayProperties("### Member Muted"),
                new ComponentSectionProperties(thumbnail)
                    .WithComponents(
                        [
                            new TextDisplayProperties($"**Member**\n{target.Username} ({target})"),
                            new TextDisplayProperties($"**Mod**\n{mod.Username} ({mod})"),
                            new TextDisplayProperties($"**Duration**\n{muteCase.Punishment}"),
                        ]
                    ),
                new TextDisplayProperties($"**Reason**\n{muteCase.Reason}"),
                new TextDisplayProperties(
                    $"-# Case #{muteCase.Id} | {target.Id} · {new Timestamp(muteCase.Date, TimestampStyle.LongDateTime)}"),
            ],
            DiscordColor.Red
        );
    }
}
