using gir.net.Application.Interfaces.Services;
using gir.net.Configurations;
using gir.net.Infra;
using gir.net.Infra.Permissions.Preconditions;
using gir.net.Infra.Services;
using gir.net.Views;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Modules.Mod;

public class ModActionsModule(
    ICaseService caseService,
    ICaseDeliveryService caseDeliveryService,
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
        await DeferResponse();

        var modTag = moderator.Username;
        var warnCase = await caseService.RecordWarnAsync(user.Id, moderator.Id, modTag, points, reason);
        var container = new WarnCaseView().CreateFrom(warnCase, user, moderator);

        var pingInPublic = false;
        if (user is GuildUser)
        {
            pingInPublic = await caseDeliveryService.TryDeliverDirectMessageAsync(user.Id, container);
        }

        await SendEditResponse(ContainerResponse(container, ephemralIfNoob: false));
        ScheduleResponseDeleteAfter(TimeSpan.FromSeconds(10));

        await caseDeliveryService.TryDeliverPublicModLogAsync(
            container,
            userToPing: pingInPublic ? null : user.Id);
    }
}
