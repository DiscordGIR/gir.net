using gir.net.Domain.Entities;
using NetCord;
using NetCord.Rest;

namespace gir.net.Views;

public sealed class UnmuteCaseView : BaseView
{
    public ComponentContainerProperties CreateFrom(Case unmuteCase, GuildUser target, GuildUser mod)
    {
        var avatarUrl = target.GetAvatarUrl()?.ToString() ?? target.DefaultAvatarUrl.ToString();
        var thumbnail = new ComponentSectionThumbnailProperties(avatarUrl);

        return CreateContainer(
            [
                new TextDisplayProperties("### Member Unmuted"),
                new ComponentSectionProperties(thumbnail)
                    .WithComponents(
                        [
                            new TextDisplayProperties($"**Member**\n{target.Username} ({target})"),
                            new TextDisplayProperties($"**Mod**\n{mod.Username} ({mod})"),
                        ]
                    ),
                new TextDisplayProperties($"**Reason**\n{unmuteCase.Reason}"),
                new TextDisplayProperties(
                    $"-# Case #{unmuteCase.Id} | {target.Id} · {new Timestamp(unmuteCase.Date, TimestampStyle.LongDateTime)}"),
            ],
            DiscordColor.Green
        );
    }
}
