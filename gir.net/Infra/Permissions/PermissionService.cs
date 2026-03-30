using gir.net.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetCord;
using NetCord.Rest;

namespace gir.net.Infra.Permissions;

public class PermissionService
{
    private readonly IReadOnlyList<PermissionLevelConfig> _levels;
    private readonly IOptions<Config> _config;
    private readonly IOptions<DiscordPermissionOptions> _permissionOptions;
    private readonly IOptions<DiscordChannels> _channels;
    private readonly ILogger<PermissionService> _logger;

    private sealed record PermissionLevelConfig(
        PermissionLevel Level,
        string Name,
        ulong? RoleId,
        Func<RestGuild?, GuildUser, bool>? Check);

    public PermissionService(
        IOptions<Config> config,
        IOptions<DiscordPermissionOptions> permissionOptions,
        IOptions<DiscordChannels> channels,
        ILogger<PermissionService> logger)
    {
        _config = config;
        _permissionOptions = permissionOptions;
        _logger = logger;
        _levels = InitializeLevels();
    }

    private IReadOnlyList<PermissionLevelConfig> InitializeLevels()
    {
        var roles = _permissionOptions.Value;
        return
        [
            new(PermissionLevel.Everyone, "Everyone and up", roles.Everyone, null),
            new(PermissionLevel.MemberPlus, "Member Plus and up", roles.MemberPlus, null),
            new(PermissionLevel.MemberPro, "Member Pro and up", roles.MemberPro, null),
            new(PermissionLevel.MemberEdition, "Member Edition and up", roles.MemberEdition, null),
            new(PermissionLevel.Genius, "Genius and up", roles.Genius, null),
            new(PermissionLevel.Moderator, "Moderator", roles.Moderator, null),
            new(PermissionLevel.Administrator, "Administrator", roles.Administrator, null),
            new(PermissionLevel.GuildOwner, "Guild Owner", null, (guild, member) =>
                guild != null && guild.OwnerId == member.Id),
            new(PermissionLevel.BotOwner, "Bot Owner", null, (_, member) =>
                _config.Value.OwnerId == member.Id),
        ];
    }

    public bool Has(GuildUser member, RestGuild? guild, PermissionLevel requiredLevel)
    {
        var memberLevel = PermissionLevel.Everyone;

        foreach (var level in _levels.OrderByDescending(l => l.Level))
        {
            if (level.Check is { } check)
            {
                if (check(guild, member))
                {
                    memberLevel = level.Level;
                    break;
                }
            }
            else if (level.RoleId is { } roleId && roleId != 0 && member.RoleIds.Contains(roleId))
            {
                memberLevel = level.Level;
                break;
            }
        }

        return memberLevel >= requiredLevel;
    }

    public MessageFlags? ShouldRespondEphemerally(GuildUser member, ulong channelId, RestGuild? guild)
    {
        if (Has(member, guild, PermissionLevel.Moderator))
            return null;

        var botCommands = _channels.Value.BotCommands;
        if (botCommands is { } id && channelId == id)
            return null;

        return MessageFlags.Ephemeral;
    }

    public async Task CheckConfiguredRolesExistAsync(RestClient restClient, CancellationToken cancellationToken = default)
    {
        var guildId = _config.Value.GuildId;
        if (guildId == 0)
            return;

        RestGuild guild;
        try
        {
            guild = await restClient.GetGuildAsync(guildId, withCounts: false, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not fetch guild {GuildId} to validate permission roles", guildId);
            return;
        }

        foreach (var level in _levels)
        {
            if (level.RoleId is not { } roleId || roleId == 0)
                continue;

            if (!guild.Roles.ContainsKey(roleId))
            {
                _logger.LogWarning(
                    "Role for \"{LevelName}\" ({RoleId}) was not found in guild {GuildName}",
                    level.Name,
                    roleId,
                    guild.Name);
            }
        }
    }
}
