using System.Net;
using gir.net.Application.Interfaces.Services;
using gir.net.Configurations;
using gir.net.Infra;
using gir.net.Infra.Permissions.Preconditions;
using gir.net.Infra.Services;
using gir.net.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Modules.Mod;

public class ModActionsModule(
    ICaseService caseService,
    ICaseDeliveryService caseDeliveryService,
    IWarnThresholdEnforcementService warnThresholdEnforcement,
    GatewayClient client,
    IOptions<Config> config,
    ILogger<ModActionsModule> logger)
    : GIRBaseCommandModule(logger)
{
    [RequirePermission<GIRContext>(PermissionLevel.Moderator)]
    [SlashCommand("warn", "Warn a user")]
    public async Task Warn(
        [SlashCommandParameter(Description = "User to warn")]
        [RequireValidInvokee<GIRContext>]
        User user,
        [SlashCommandParameter(Description = "Points to warn for", MinValue = 1)] int points,
        [SlashCommandParameter(Description = "Reason for warning")] string reason
    )
    {
        var moderator = (GuildUser)Context.Interaction.User;
        await Responder.DeferAsync();

        var modTag = moderator.Username;
        var warnResult = await caseService.RecordWarnAsync(user.Id, moderator.Id, modTag, points, reason);
        var container = new WarnCaseView().CreateFrom(
            warnResult.Case, user, moderator, warnResult.CurrentWarnPoints);

        var guildId = config.Value.GuildId;
        var guildName = Context.Guild!.Name;

        var enforcement = await warnThresholdEnforcement.EnforceAsync(
            guildId, guildName, user, moderator, warnResult, container);

        await Responder.ReplyAsync(container, ReplyOptions.Public);
        Responder.ScheduleDeleteAfter(TimeSpan.FromSeconds(10));

        await caseDeliveryService.TryDeliverPublicModLogAsync(
            container,
            userToPing: enforcement.DmDelivered ? null : user.Id);

        if (enforcement.FollowUp is { } followUp)
            await caseDeliveryService.TryDeliverPublicModLogAsync(followUp.Container);
    }

    [RequirePermission<GIRContext>(PermissionLevel.Moderator)]
    [SlashCommand("kick", "Kick a user")]
    public async Task Kick(
        [SlashCommandParameter(Description = "User to kick")]
        [RequireValidInvokee<GIRContext>]
        GuildUser user,
        [SlashCommandParameter(Description = "Reason for kick")] string reason
    )
    {
        var moderator = (GuildUser)Context.Interaction.User;
        await Responder.DeferAsync();

        var guildName = Context.Guild!.Name;
        var kickCase = await caseService.RecordKickAsync(user.Id, moderator.Id, moderator.Username, reason);
        await caseService.MarkWarnKickedAsync(user.Id);

        var container = new KickCaseView().CreateFrom(kickCase, user, moderator);

        await caseDeliveryService.TryDeliverDirectMessageAsync(
            user.Id,
            container,
            $"You were kicked from {guildName}.");

        try
        {
            await client.Rest.KickGuildUserAsync(config.Value.GuildId, user.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to kick user {UserId}", user.Id);
            await Responder.ReplyErrorAsync("Failed to kick that user. Check my permissions and role hierarchy.");
            return;
        }

        await Responder.ReplyAsync(container, ReplyOptions.Public);
        Responder.ScheduleDeleteAfter(TimeSpan.FromSeconds(10));

        await caseDeliveryService.TryDeliverPublicModLogAsync(container);
    }

    [RequirePermission<GIRContext>(PermissionLevel.Moderator)]
    [SlashCommand("ban", "Ban a user")]
    public async Task Ban(
        [SlashCommandParameter(Description = "User to ban")]
        [RequireValidInvokee<GIRContext>]
        User user,
        [SlashCommandParameter(Description = "Reason for ban")] string reason
    )
    {
        var moderator = (GuildUser)Context.Interaction.User;

        if (user is not GuildUser)
        {
            try
            {
                await client.Rest.GetGuildBanAsync(config.Value.GuildId, user.Id);
                await Responder.ReplyErrorAsync("That user is already banned!");
                return;
            }
            catch (RestException ex) when (IsNotFound(ex))
            {
            }
        }

        await Responder.DeferAsync();

        var guildName = Context.Guild!.Name;
        var banCase = await caseService.RecordBanAsync(user.Id, moderator.Id, moderator.Username, reason);
        var container = new BanCaseView().CreateFrom(banCase, user, moderator);

        if (user is GuildUser)
        {
            await caseDeliveryService.TryDeliverDirectMessageAsync(
                user.Id,
                container,
                BuildBanDmMessage(guildName));
        }

        try
        {
            await client.Rest.BanGuildUserAsync(config.Value.GuildId, user.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to ban user {UserId}", user.Id);
            await Responder.ReplyErrorAsync("Failed to ban that user. Check my permissions and role hierarchy.");
            return;
        }

        await Responder.ReplyAsync(container, ReplyOptions.Public);
        Responder.ScheduleDeleteAfter(TimeSpan.FromSeconds(10));

        await caseDeliveryService.TryDeliverPublicModLogAsync(container);
    }

    [RequirePermission<GIRContext>(PermissionLevel.Moderator)]
    [SlashCommand("unban", "Unban a user")]
    public async Task Unban(
        [SlashCommandParameter(Description = "User to unban")] User user,
        [SlashCommandParameter(Description = "Reason for unban")] string reason
    )
    {
        var moderator = (GuildUser)Context.Interaction.User;

        if (user is GuildUser)
        {
            await Responder.ReplyErrorAsync("You can't unban someone already in the server!");
            return;
        }

        try
        {
            await client.Rest.GetGuildBanAsync(config.Value.GuildId, user.Id);
        }
        catch (RestException ex) when (IsNotFound(ex))
        {
            await Responder.ReplyErrorAsync("That user isn't banned!");
            return;
        }

        await Responder.DeferAsync();

        try
        {
            await client.Rest.UnbanGuildUserAsync(config.Value.GuildId, user.Id);
        }
        catch (RestException ex) when (IsNotFound(ex))
        {
            await Responder.ReplyErrorAsync($"{user} is not banned.");
            return;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to unban user {UserId}", user.Id);
            await Responder.ReplyErrorAsync("Failed to unban that user. Check my permissions.");
            return;
        }

        var unbanCase = await caseService.RecordUnbanAsync(user.Id, moderator.Id, moderator.Username, reason);
        var container = new UnbanCaseView().CreateFrom(unbanCase, user, moderator);

        await Responder.ReplyAsync(container, ReplyOptions.Public);
        Responder.ScheduleDeleteAfter(TimeSpan.FromSeconds(10));

        await caseDeliveryService.TryDeliverPublicModLogAsync(container);
    }

    private static bool IsNotFound(RestException ex) =>
        ex.StatusCode == HttpStatusCode.NotFound;

    private string BuildBanDmMessage(string guildName)
    {
        var message = $"You have been banned from {guildName}.";
        var appealUrl = config.Value.BanAppealUrl;
        if (!string.IsNullOrWhiteSpace(appealUrl))
        {
            message +=
                $"\n\nIf you would like to appeal your ban, please fill out this form: <{appealUrl}>";
        }

        return message;
    }
}
