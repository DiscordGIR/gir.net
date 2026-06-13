using gir.net.Application.Interfaces.Services;
using gir.net.Configurations;
using gir.net.Infra;
using gir.net.Infra.Permissions.Preconditions;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Modules.Mod;

public class ModActionsModule(ICaseService caseService, ILogger<ModActionsModule> logger)
    : GIRBaseCommandModule(logger)
{
    [RequirePermission<GIRContext>(PermissionLevel.Moderator)]
    [SlashCommand("warn", "Warn a user")]
    public async Task<InteractionMessageProperties> Warn(
        [SlashCommandParameter(Description = "User to warn")] User user,
        [SlashCommandParameter(Description = "Points to warn for", MinValue = 1)] int points,
        [SlashCommandParameter(Description = "Reason for warning")] string reason
    )
    {
        if (Context.Interaction.User is not GuildUser guildUser)
            return ErrorResponse("This command can only be used in a server.");

        var modTag = guildUser.Username;

        try
        {
            var warnCase = await caseService.RecordWarnAsync(user.Id, guildUser.Id, modTag, points, reason);
            return SuccessResponse(
                "Warning recorded",
                $"Case #{warnCase.Id} · {warnCase.Punishment} · {user}");
        }
        catch (ArgumentException ex)
        {
            return ErrorResponse(ex.Message);
        }
    }
}