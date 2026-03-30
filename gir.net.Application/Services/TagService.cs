using System.IO;
using System.Threading.Tasks;
using gir.net.Application.Interfaces.Services;
using gir.net.Domain.Entities;
using gir.net.Domain.Interfaces.Repositories;

namespace gir.net.Application.Services;

public class TagService(ITagRepository tagRepository, IImageStorageService storageService) : ITagService
{
    public Task<Tag?> GetTagAsync(string name)
    {
        return tagRepository.GetTagAsync(name);
    }

    public Task AddTagAsync(Tag tag)
    {
        return tagRepository.AddTagAsync(tag);
    }

    public async Task AddTagWithImageAsync(Tag tag, Stream imageStream, string fileName, string contentType)
    {
        string imageUrl = await storageService.UploadImageAsync(imageStream, fileName, contentType);
        tag.ImageUrl = imageUrl;
        await tagRepository.AddTagAsync(tag);
    }

    public Task UpdateTagAsync(Tag tag)
    {
        return tagRepository.UpdateTagAsync(tag);
    }

    public Task DeleteTagAsync(string name)
    {
        return tagRepository.DeleteTagAsync(name);
    }

    public Task MarkTagUsage(Tag tag)
    {
        tag.UseCount += 1;
        return tagRepository.UpdateTagAsync(tag);
    }
}
