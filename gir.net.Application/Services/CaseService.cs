using gir.net.Application.Interfaces.Services;
using gir.net.Domain.Entities;
using gir.net.Domain.Interfaces.Repositories;

namespace gir.net.Application.Services;

public class CaseService(ICaseRepository caseRepository) : ICaseService
{
    public async Task<Case> RecordWarnAsync(ulong userId, ulong modId, string modTag, int points, string reason)
    {
        var entity = ModerationCaseFactory.CreateWarn(userId, modId, modTag, points, reason);
        await caseRepository.AddCaseAsync(entity);
        return entity;
    }

    public Task<Case?> GetByIdAsync(int id) => caseRepository.GetByIdAsync(id);

    public Task AddCaseAsync(Case entity) => caseRepository.AddCaseAsync(entity);

    public Task UpdateCaseAsync(Case entity) => caseRepository.UpdateCaseAsync(entity);

    public Task<IEnumerable<Case>> GetCasesByUserIdAsync(ulong userId, int take = 50) =>
        caseRepository.GetCasesByUserIdAsync(userId, take);
}
