using Contracts.File;
using Domain.Core.Primitives;
using Domain.Core.Utility;
using Domain.Enumeration;
using Infrastructure.Files;
using Microsoft.AspNetCore.Http;

namespace Application.Services.File
{
    public class FileService : IFileService
    {
        private readonly IFilesProvider filesProvider;

        public FileService(IFilesProvider filesProvider) => this.filesProvider = filesProvider;

        public async Task<Result<bool>> UploadCertificate(AppendCertificateRequest request, CancellationToken cancellationToken)
        {
            if (request.blank is not null)
            {
                var mimeTypeResult = filesProvider.getMimeType(request.blank);
                if (!mimeTypeResult.isSuccess) return new Result<bool>(mimeTypeResult.error);
                if (!Ensure.isValidCertificateBlankMimeType(mimeTypeResult.value)) return new Result<bool>(CommonErrors.File.UnsupportedMediaType);

                var uploadResult = await filesProvider.UploadCertificateBlankAsync(request.blank, cancellationToken);
                if (!uploadResult.isSuccess) return new Result<bool>(uploadResult.error);
            }
            return await filesProvider.UploadCertificateConfigAsync(request, cancellationToken);
        }

        public async Task UploadLegalFilesAsync(AppendLegalFilesRequest request, CancellationToken cancellationToken)
        {
            if (request.regulations is not null && ValidateLegalFile(request.regulations).isSuccess)
                await filesProvider.UploadLegalFileAsync(request.regulations, "regulations.pdf", cancellationToken);

            if (request.privacyPolicy is not null && ValidateLegalFile(request.privacyPolicy).isSuccess)
                await filesProvider.UploadLegalFileAsync(request.privacyPolicy, "privacy_policy.pdf", cancellationToken);

            if (request.youthConsent is not null && ValidateLegalFile(request.youthConsent).isSuccess)
                await filesProvider.UploadLegalFileAsync(request.youthConsent, "y_consent.pdf", cancellationToken);

            if (request.adultConsent is not null && ValidateLegalFile(request.adultConsent).isSuccess)
                await filesProvider.UploadLegalFileAsync(request.adultConsent, "a_consent.pdf", cancellationToken);
        }

        private Result<string> ValidateLegalFile(IFormFile file)
        {
            var MimeResult = filesProvider.getMimeType(file);
            if (!MimeResult.isSuccess) return MimeResult;

            if (Ensure.isValidLegalMimeType(MimeResult.value))
                return new Result<string>();
            
            return new Result<string>(CommonErrors.File.UnsupportedMediaType);
        }
    }
}
