using gir.net.Application;
using gir.net.Domain.Entities;

namespace gir.net.Application.Interfaces.Services;

public interface ICaseService
{
    Task<WarnRecordResult> RecordWarnAsync(ulong userId, ulong modId, string modTag, int points, string reason);
    Task<Case> RecordKickAsync(ulong userId, ulong modId, string modTag, string reason);
    Task<Case> RecordBanAsync(ulong userId, ulong modId, string modTag, string reason);
    Task MarkWarnKickedAsync(ulong userId);

    Task<Case?> GetByIdAsync(int id);
    Task AddCaseAsync(Case entity);
    Task UpdateCaseAsync(Case entity);
    Task<IEnumerable<Case>> GetCasesByUserIdAsync(ulong userId, int take = 50);
}
