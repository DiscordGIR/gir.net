using gir.net.Domain.Entities;

namespace gir.net.Application.Interfaces.Services;

public interface ITagService
{
    Task<Tag?> GetTagAsync(string name);
    Task AddTagAsync(Tag tag);
    Task UpdateTagAsync(Tag tag);
    Task DeleteTagAsync(string name);
}
