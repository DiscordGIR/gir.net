using gir.net.Domain.Entities;

namespace gir.net.Application.Interfaces.Services;

public interface IFilterService
{
    public Task AddFilterWordAsync(FilterWord filterWord);
}