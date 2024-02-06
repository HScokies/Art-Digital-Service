using Api.Controllers.Base;
using Application.Services.Participant;
using Contracts.File;
using Domain.Core.Primitives;
using Domain.Core.Utility;
using Domain.Enumeration;
using Infrastructure.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class FilesController : ApiController
    {
        private readonly IParticipantService participantService;
        private readonly IFilesProvider filesProvider;

        public FilesController(ILogger<ApiController> logger, IParticipantService participantService, IFilesProvider filesProvider) : base(logger)
        {
            this.participantService = participantService;
            this.filesProvider = filesProvider;
        }

        [HttpGet("user-uploaded/{filename}"), Authorize(Roles = Roles.Permissions.readUsers)]
        public async Task<IActionResult> GetUserFile(string filename, string? displayedName, CancellationToken cancellationToken)
        {
            var res = await participantService.GetParticipantFileAsync(filename, cancellationToken);
            if (!res.isSuccess)
                return Problem(res.error);

            var filedata = res.value;
            return File(filedata.fileStream, filedata.contentType, displayedName is null? filename : displayedName);
        }

        [HttpGet("legal/{filename}")]
        public async Task<IActionResult> GetLegalFile(string filename, string? displayedName, CancellationToken cancellationToken)
        {
            var res = await filesProvider.DownloadLegalFileAsync(filename, cancellationToken);
            if (!res.isSuccess)
                return Problem(res.error);
            var filedata = res.value;

            return File(filedata.fileStream, filedata.contentType, displayedName is null ? filename : displayedName);
        }

        [HttpPatch("legal"), Authorize(Roles = Roles.Permissions.utilsAccess)]
        public async Task<IActionResult> AppendLegalFiles([FromForm] AppendLegalFilesRequest request, CancellationToken cancellationToken)
        {

            if (request.regulations is not null)
                await filesProvider.TryUploadFileAsync(request.regulations, "regulations.pdf", cancellationToken);
            
            if (request.privacyPolicy is not null)
                await filesProvider.TryUploadFileAsync(request.privacyPolicy, "privacy_policy.pdf", cancellationToken);

            if (request.youthConsent is not null)
                await filesProvider.TryUploadFileAsync(request.youthConsent, "y_consent.pdf", cancellationToken);

            if (request.adultConsent is not null)
                await filesProvider.TryUploadFileAsync(request.adultConsent, "a_consent.pdf", cancellationToken);

            return NoContent();
        }

        [HttpGet("certificate-config"), Authorize(Roles = Roles.Permissions.utilsAccess)]
        public async Task<IActionResult> GetCertificateConfig(CancellationToken cancellationToken)
        {
            var Result = await filesProvider.GetCertificateConfigAsync(cancellationToken);
            return Result.isSuccess ? Ok(Result.value) : Problem(Result.error);
        }

        [HttpGet("certificate"), Authorize(Roles = Roles.Permissions.utilsAccess)]
        public async Task<IActionResult> GetCertificateBlank(CancellationToken cancellationToken)
        {
            var Result = await filesProvider.DownloadCertificateBlank(cancellationToken);
            if (!Result.isSuccess)
                return Problem(Result.error);
            return File(Result.value.fileStream, Result.value.contentType, Result.value.fileName);
        }
    }
}
