using gir.net.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;

namespace gir.net.Infra.Services;

public class CaseDeliveryService(
    GatewayClient client,
    IOptions<DiscordChannels> channels,
    ILogger<CaseDeliveryService> logger) : ICaseDeliveryService
{
    public async Task<bool> TryDeliverDirectMessageAsync(
        ulong targetUserId,
        ComponentContainerProperties container,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var dmChannel = await client.Rest.GetDMChannelAsync(targetUserId, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            await dmChannel.SendMessageAsync(ToMessage(container), cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to deliver case notification DM to user {UserId}", targetUserId);
            return false;
        }
    }

    public async Task<bool> TryDeliverPublicModLogAsync(
        ComponentContainerProperties container,
        ulong? userToPing = null,
        CancellationToken cancellationToken = default)
    {
        var channelId = channels.Value.PublicModLogs;
        if (channelId is not { } id)
        {
            logger.LogWarning("PublicModLogs channel is not configured; skipping public mod log delivery");
            return false;
        }

        try
        {
            await client.Rest.SendMessageAsync(id, ToMessage(container, userToPing),
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to post case notification to public mod logs channel {ChannelId}", id);
            return false;
        }
    }

    private static MessageProperties ToMessage(ComponentContainerProperties container, ulong? mentionUserId = null)
    {
        var message = new MessageProperties()
            .WithComponents([container])
            .WithFlags(MessageFlags.IsComponentsV2)
            .WithAllowedMentions(mentionUserId is { } userId
                ? new AllowedMentionsProperties().WithAllowedUsers([userId])
                : AllowedMentionsProperties.None);

        return message;
    }
}
