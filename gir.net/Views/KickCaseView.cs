using gir.net.Domain.Entities;
using NetCord;
using NetCord.Rest;
using Color = System.Drawing.Color;

namespace gir.net.Views;

public sealed class KickCaseView : BaseView
{
    public ComponentContainerProperties CreateFrom(Case kickCase, NetCord.User target, GuildUser mod)
    {
        var avatarUrl = target.GetAvatarUrl()?.ToString() ?? target.DefaultAvatarUrl.ToString();
        var thumbnail = new ComponentSectionThumbnailProperties(avatarUrl);

        return CreateContainer(
            [
                new TextDisplayProperties("### Member Kicked"),
                new ComponentSectionProperties(thumbnail)
                    .WithComponents(
                        [
                            new TextDisplayProperties($"**Member**\n{target.Username} ({target})"),
                            new TextDisplayProperties($"**Mod**\n{mod.Username} ({mod})"),
                        ]
                    ),
                new TextDisplayProperties($"**Reason**\n{kickCase.Reason}"),
                new TextDisplayProperties(
                    $"-# Case #{kickCase.Id} | {target.Id} · {new Timestamp(kickCase.Date, TimestampStyle.LongDateTime)}"),
            ],
            Color.Green
        );
    }
}
