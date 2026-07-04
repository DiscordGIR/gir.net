using gir.net.Application;
using gir.net.Application.Interfaces.Services;
using gir.net.Configurations;
using gir.net.Domain.Entities;
using gir.net.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;

namespace gir.net.Infra.Services;

public interface IWarnThresholdEnforcementService
{
    Task<WarnEnforcementResult> EnforceAsync(
        ulong guildId,
        string guildName,
        NetCord.User target,
        GuildUser mod,
        WarnRecordResult warn,
        ComponentContainerProperties warnContainer,
        CancellationToken cancellationToken = default);
}

public sealed record WarnEnforcementResult(bool DmDelivered, CaseFollowUp? FollowUp);

public sealed record CaseFollowUp(Case Case, ComponentContainerProperties Container);

public class WarnThresholdEnforcementService(
    GatewayClient client,
    ICaseService caseService,
    ICaseDeliveryService caseDeliveryService,
    IOptions<Config> config,
    ILogger<WarnThresholdEnforcementService> logger) : IWarnThresholdEnforcementService
{
    public const int KickThreshold = 400;
    public const int BanThreshold = 600;
    public const string KickReason = "400 or more warn points reached.";
    public const string BanReason = "600 or more warn points reached.";

    public async Task<WarnEnforcementResult> EnforceAsync(
        ulong guildId,
        string guildName,
        NetCord.User target,
        GuildUser mod,
        WarnRecordResult warn,
        ComponentContainerProperties warnContainer,
        CancellationToken cancellationToken = default)
    {
        var isMember = target is GuildUser;
        var points = warn.CurrentWarnPoints;
        CaseFollowUp? followUp = null;
        bool dmDelivered;

        if (points >= BanThreshold)
        {
            dmDelivered = await caseDeliveryService.TryDeliverDirectMessageAsync(
                target.Id,
                warnContainer,
                BuildBanDirectMessage(guildName),
                cancellationToken);

            var banCase = await caseService.RecordBanAsync(target.Id, mod.Id, mod.Username, BanReason);
            followUp = new CaseFollowUp(banCase, new BanCaseView().CreateFrom(banCase, target, mod));

            try
            {
                await client.Rest.BanGuildUserAsync(guildId, target.Id, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to ban user {UserId} after reaching warn threshold", target.Id);
            }
        }
        else if (points >= KickThreshold && !warn.WasWarnKicked && isMember)
        {
            await caseService.MarkWarnKickedAsync(target.Id);

            dmDelivered = await caseDeliveryService.TryDeliverDirectMessageAsync(
                target.Id,
                warnContainer,
                $"You were kicked from {guildName} for reaching 400 or more points. Please note that you will be banned at 600 points.",
                cancellationToken);

            var kickCase = await caseService.RecordKickAsync(target.Id, mod.Id, mod.Username, KickReason);
            followUp = new CaseFollowUp(kickCase, new KickCaseView().CreateFrom(kickCase, target, mod));

            try
            {
                await client.Rest.KickGuildUserAsync(guildId, target.Id, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to kick user {UserId} after reaching warn threshold", target.Id);
            }
        }
        else if (isMember)
        {
            dmDelivered = await caseDeliveryService.TryDeliverDirectMessageAsync(
                target.Id,
                warnContainer,
                $"You were warned in {guildName}. Please note that you will be kicked at 400 points and banned at 600 points.",
                cancellationToken);
        }
        else
        {
            dmDelivered = false;
        }

        return new WarnEnforcementResult(dmDelivered, followUp);
    }

    private string BuildBanDirectMessage(string guildName)
    {
        var message = $"You were banned from {guildName} for reaching 600 or more points.";
        var appealUrl = config.Value.BanAppealUrl;
        if (!string.IsNullOrWhiteSpace(appealUrl))
        {
            message +=
                $"\n\nIf you would like to appeal your ban, please fill out this form: <{appealUrl}>";
        }

        return message;
    }
}
