using Domain.Core.Primitives;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Files
{
    public interface IFilesService
    {
        public Task<Result<string>> UploadFileAsync(IFormFile file);
        public Task<Result<MemoryStream>> DownloadFileAsync(string id, string? displayedName);
        public Result<string> getMimeType(IFormFile file);
    }
}
