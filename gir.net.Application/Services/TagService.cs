using gir.net.Application.Interfaces.Services;
using gir.net.Domain.Entities;
using gir.net.Domain.Interfaces.Repositories;

namespace gir.net.Application.Services;

public class TagService(ITagRepository tagRepository) : ITagService
{
    public Task<Tag?> GetTagAsync(string name)
    {
        return tagRepository.GetTagAsync(name);
    }

    public Task AddTagAsync(Tag tag)
    {
        return tagRepository.AddTagAsync(tag);
    }

    public Task UpdateTagAsync(Tag tag)
    {
        return tagRepository.UpdateTagAsync(tag);
    }

    public Task DeleteTagAsync(string name)
    {
        return tagRepository.DeleteTagAsync(name);
    }
}
