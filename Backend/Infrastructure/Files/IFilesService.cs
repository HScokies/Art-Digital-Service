using Domain.Core.Primitives;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Files
{
    public interface IFilesService
    {
        public Task<Result<string>> UploadUserFileAsync(IFormFile file, CancellationToken cancellationToken);
        public void DropUserFile(string fileName);
        public Task<Result<MemoryStream>> DownloadFileAsync(string id, string? displayedName, CancellationToken cancellationToken);
        public Result<string> getMimeType(IFormFile file);
    }
}
