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
    public MessageFlags? WhisperFlagIfNoPermissions()
    {
        if (Interaction.User is not GuildUser guildUser)
            return MessageFlags.Ephemeral;

        if (Guild is not RestGuild restGuild)
            return MessageFlags.Ephemeral;

        return permissionService.ShouldRespondEphemerally(guildUser, Channel.Id, restGuild)
            ? MessageFlags.Ephemeral
            : null;
    }
}