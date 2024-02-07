using Contracts.File;
using Domain.Core.Primitives;

namespace Application.Services.File
{
    public interface IFileService
    {
        Task UploadLegalFilesAsync(AppendLegalFilesRequest request, CancellationToken cancellationToken);
        Task<Result<bool>> UploadCertificate(AppendCertificateRequest request, CancellationToken cancellationToken);
    }
}
