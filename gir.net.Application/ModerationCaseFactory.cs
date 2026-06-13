using gir.net.Domain.Entities;

namespace gir.net.Application;

public static class ModerationCaseFactory
{
    public static Case CreateWarn(ulong userId, ulong modId, string modTag, int points, string reason)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(points, 0);
        ArgumentException.ThrowIfNullOrWhiteSpace(modTag);
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        var trimmedReason = reason.Trim();
        var punishment = points == 1 ? "1 point" : $"{points} points";

        return new Case
        {
            Type = CaseType.Warn,
            Date = DateTime.UtcNow,
            EndDate = null,
            UserId = userId,
            ModId = modId,
            ModTag = modTag.Trim(),
            Reason = trimmedReason,
            Punishment = punishment,
            Lifted = false,
            LiftedByTag = null,
            LiftedById = null,
            LiftedReason = null,
            LiftedDate = null
        };
    }
}
