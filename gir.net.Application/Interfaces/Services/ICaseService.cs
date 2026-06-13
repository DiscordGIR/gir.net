using gir.net.Domain.Entities;

namespace gir.net.Application.Interfaces.Services;

public interface ICaseService
{
    Task<Case> RecordWarnAsync(ulong userId, ulong modId, string modTag, int points, string reason);

    Task<Case?> GetByIdAsync(int id);
    Task AddCaseAsync(Case entity);
    Task UpdateCaseAsync(Case entity);
    Task<IEnumerable<Case>> GetCasesByUserIdAsync(ulong userId, int take = 50);
}
