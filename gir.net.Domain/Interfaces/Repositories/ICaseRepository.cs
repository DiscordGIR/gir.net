using gir.net.Domain.Entities;

namespace gir.net.Domain.Interfaces.Repositories;

public interface ICaseRepository
{
    Task<Case?> GetByIdAsync(int id);
    Task AddCaseAsync(Case entity);
    Task UpdateCaseAsync(Case entity);
    Task<IEnumerable<Case>> GetCasesByUserIdAsync(ulong userId, int take = 50);
}
