using Contracts.File;
using Contracts.Participant;
using Domain.Core.Primitives;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Files
{
    public interface IFilesProvider
    {
        Task<Result<string>> UploadUserFileAsync(IFormFile file, CancellationToken cancellationToken);
        Task<Result<string>> UploadLegalFileAsync(IFormFile file, string fileName, CancellationToken cancellationToken);
        Task<Result<string>> UploadCertificateBlankAsync(IFormFile file, CancellationToken cancellationToken);
        Task<Result<bool>> UploadCertificateConfigAsync(AppendCertificateRequest request, CancellationToken cancellationToken);
        void DropUserFile(string fileName);
        Task<Result<FileResponse>> DownloadUserFileAsync(string fileName, CancellationToken cancellationToken);
        Result<string> getMimeType(IFormFile file);
        Task<Result<FileResponse>> DownloadLegalFileAsync(string fileName, CancellationToken cancellationToken);
        Task<Result<FileResponse>> DownloadCertificateAsync(GetCertificateRequest request, CancellationToken cancellationToken);
        Task<Result<CertificateModel>> GetCertificateConfigAsync(CancellationToken cancellationToken);
        Task<Result<FileResponse>> DownloadCertificateBlankAsync(CancellationToken cancellationToken);
    }
}
