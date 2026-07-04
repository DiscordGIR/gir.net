using NetCord;
using NetCord.Rest;

namespace gir.net.Infra.Permissions;

public class GuildMemberHierarchyService
{
    public HierarchyCheckResult CanActOn(GuildUser actor, User target, ulong botUserId, RestGuild guild)
    {
        if (target.Id == actor.Id)
            return HierarchyCheckResult.Denied("You can't call that on yourself.");

        if (target.Id == botUserId)
            return HierarchyCheckResult.Denied("You can't call that on me :(");

        if (target is GuildUser targetMember)
        {
            var actorPosition = GetHighestRolePosition(guild, actor);
            var targetPosition = GetHighestRolePosition(guild, targetMember);

            if (actorPosition <= targetPosition)
                return HierarchyCheckResult.Denied($"{targetMember}'s top role is the same or higher than yours!");
        }

        return HierarchyCheckResult.Allowed();
    }

    private static int GetHighestRolePosition(RestGuild guild, GuildUser member) =>
        member.RoleIds
            .Select(id => guild.Roles.TryGetValue(id, out var role) ? role.RawPosition : 0)
            .DefaultIfEmpty(0)
            .Max();
}

public readonly record struct HierarchyCheckResult(bool IsAllowed, string? FailureMessage)
{
    public static HierarchyCheckResult Allowed() => new(true, null);

    public static HierarchyCheckResult Denied(string message) => new(false, message);
}
