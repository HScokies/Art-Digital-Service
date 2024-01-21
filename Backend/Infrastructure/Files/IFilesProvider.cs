using Contracts.File;
using Domain.Core.Primitives;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Files
{
    public interface IFilesProvider
    {
        public Task<Result<string>> UploadUserFileAsync(IFormFile file, CancellationToken cancellationToken);
        public void DropUserFile(string fileName);
        public Task<Result<FileResponse>> DownloadUserFileAsync(string fileName, CancellationToken cancellationToken);
        public Result<string> getMimeType(IFormFile file);
    }
}
