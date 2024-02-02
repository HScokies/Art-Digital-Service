using Contracts.File;
using Domain.Core.Primitives;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Files
{
    public interface IFilesProvider
    {
        Task<Result<string>> UploadUserFileAsync(IFormFile file, CancellationToken cancellationToken);
        void DropUserFile(string fileName);
        Task<Result<FileResponse>> DownloadUserFileAsync(string fileName, CancellationToken cancellationToken);
        Result<string> getMimeType(IFormFile file);
        Task<Result<FileResponse>> DownloadLegalFileAsync(string fileName, CancellationToken cancellationToken);
    }
}
