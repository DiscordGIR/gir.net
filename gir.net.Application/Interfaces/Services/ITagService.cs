using gir.net.Domain.Entities;

namespace gir.net.Application.Interfaces.Services;

public interface ITagService
{
    Task<Tag?> GetTagAsync(string name);
    Task AddTagAsync(Tag tag);
    Task AddTagWithImageAsync(Tag tag, Stream imageStream, string fileName, string contentType);
    Task UpdateTagAsync(Tag tag);
    Task DeleteTagAsync(string name);
    Task MarkTagUsage(Tag tag);
}
