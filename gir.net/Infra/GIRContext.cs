using gir.net.Infra.Permissions;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Infra;

public class GIRContext(
    ApplicationCommandInteraction interaction,
    GatewayClient client,
    PermissionService permissionService)
    : ApplicationCommandContext(interaction, client)
{
    public bool ShouldWhisperIfNoPermissions()
    {
        if (Interaction.User is not GuildUser guildUser)
            return true;

        if (Guild is not RestGuild restGuild)
            return true;

        return permissionService.ShouldRespondEphemerally(guildUser, Channel.Id, restGuild);
    }
    
    public GuildUser BotGuildUser => Guild!.Users[Client.Id];
}