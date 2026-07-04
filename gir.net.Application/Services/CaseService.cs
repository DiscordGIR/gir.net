using gir.net.Application.Interfaces.Services;
using gir.net.Domain.Entities;
using gir.net.Domain.Interfaces.Repositories;

namespace gir.net.Application.Services;

public class CaseService(ICaseRepository caseRepository, IUserRepository userRepository) : ICaseService
{
    public async Task<WarnRecordResult> RecordWarnAsync(
        ulong userId, ulong modId, string modTag, int points, string reason)
    {
        var user = await userRepository.GetOrCreateAsync(userId);
        var wasWarnKicked = user.WasWarnKicked;

        var entity = ModerationCaseFactory.CreateWarn(userId, modId, modTag, points, reason);
        await caseRepository.AddCaseAsync(entity);

        var currentWarnPoints = await userRepository.AddWarnPointsAsync(userId, points);
        return new WarnRecordResult(entity, currentWarnPoints, wasWarnKicked);
    }

    public async Task<Case> RecordKickAsync(ulong userId, ulong modId, string modTag, string reason)
    {
        var entity = ModerationCaseFactory.CreateKick(userId, modId, modTag, reason);
        await caseRepository.AddCaseAsync(entity);
        return entity;
    }

    public async Task<Case> RecordBanAsync(ulong userId, ulong modId, string modTag, string reason)
    {
        var entity = ModerationCaseFactory.CreateBan(userId, modId, modTag, reason);
        await caseRepository.AddCaseAsync(entity);
        return entity;
    }

    public async Task<Case> RecordUnbanAsync(ulong userId, ulong modId, string modTag, string reason)
    {
        var entity = ModerationCaseFactory.CreateUnban(userId, modId, modTag, reason);
        await caseRepository.AddCaseAsync(entity);
        return entity;
    }

    public async Task<Case> RecordMuteAsync(
        ulong userId, ulong modId, string modTag, string reason, DateTime endDate, string durationLabel)
    {
        var entity = ModerationCaseFactory.CreateMute(userId, modId, modTag, reason, endDate, durationLabel);
        await caseRepository.AddCaseAsync(entity);
        return entity;
    }

    public async Task<Case> RecordUnmuteAsync(ulong userId, ulong modId, string modTag, string reason)
    {
        var entity = ModerationCaseFactory.CreateUnmute(userId, modId, modTag, reason);
        await caseRepository.AddCaseAsync(entity);
        return entity;
    }

    public Task MarkWarnKickedAsync(ulong userId) =>
        userRepository.SetWasWarnKickedAsync(userId);

    public Task<Case?> GetByIdAsync(int id) => caseRepository.GetByIdAsync(id);

    public Task AddCaseAsync(Case entity) => caseRepository.AddCaseAsync(entity);

    public Task UpdateCaseAsync(Case entity) => caseRepository.UpdateCaseAsync(entity);

    public Task<IEnumerable<Case>> GetCasesByUserIdAsync(ulong userId, int take = 50) =>
        caseRepository.GetCasesByUserIdAsync(userId, take);
}
