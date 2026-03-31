using gir.net.Domain.Entities;

namespace gir.net.Domain.Interfaces.Repositories;

public interface ITagRepository
{
    Task<Tag?> GetTagAsync(string name);
    Task AddTagAsync(Tag tag);
    Task UpdateTagAsync(Tag tag);
    Task DeleteTagAsync(string name);
    Task<IEnumerable<Tag>> SearchTagsAsync(string searchText);
}
