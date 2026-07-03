using gir.net.Domain.Entities;
using NetCord;
using NetCord.Rest;
using Color = System.Drawing.Color;

namespace gir.net.Views;

public sealed class WarnCaseView : BaseView
{
    public ComponentContainerProperties CreateFrom(Case warnCase, NetCord.User target, GuildUser mod)
    {
        var avatarUrl = target.GetAvatarUrl()?.ToString() ?? target.DefaultAvatarUrl.ToString();
        var thumbnail = new ComponentSectionThumbnailProperties(avatarUrl);

        return CreateContainer(
            [
                new TextDisplayProperties("### Member Warned"),
                new ComponentSectionProperties(thumbnail)
                    .WithComponents(
                        [
                            new TextDisplayProperties($"**Member**\n{target.Username} ({target})"),
                            new TextDisplayProperties($"**Mod**\n{mod.Username} ({mod})"),
                            new TextDisplayProperties($"**Increase**\n{warnCase.Punishment}"),
                        ]
                    ),
                new TextDisplayProperties($"**Reason**\n{warnCase.Reason}"),
                new TextDisplayProperties(
                    $"-# Case #{warnCase.Id} | {target.Id} · {new Timestamp(warnCase.Date, TimestampStyle.LongDateTime)}"),
            ],
            Color.Orange
        );
    }
}
