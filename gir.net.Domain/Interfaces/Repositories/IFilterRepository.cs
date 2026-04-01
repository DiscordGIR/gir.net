using gir.net.Domain.Entities;

namespace gir.net.Domain.Interfaces.Repositories;

public interface IFilterRepository
{
    Task AddFilterWordAsync(FilterWord filterWord);
}