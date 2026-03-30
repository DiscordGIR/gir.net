using System.IO;
using System.Threading.Tasks;

namespace gir.net.Application.Interfaces.Services;

public interface IImageStorageService
{
    Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType);
}
