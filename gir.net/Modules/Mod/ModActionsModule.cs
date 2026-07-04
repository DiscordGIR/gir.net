using gir.net.Application.Interfaces.Services;
using gir.net.Infra;
using gir.net.Infra.Permissions.Preconditions;
using gir.net.Infra.Services;
using gir.net.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using gir.net.Configurations;

namespace gir.net.Modules.Mod;

public class ModActionsModule(
    ICaseService caseService,
    ICaseDeliveryService caseDeliveryService,
    IWarnThresholdEnforcementService warnThresholdEnforcement,
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
        var guildName = Context.Guild?.Name ?? "the server";

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
}
