using Api.Controllers.Base;
using Application.Services.File;
using Application.Services.Participant;
using Contracts.File;
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
        private readonly IFileService fileService;

        public FilesController(ILogger<ApiController> logger, IParticipantService participantService, IFilesProvider filesProvider, IFileService fileService) : base(logger)
        {
            this.participantService = participantService;
            this.filesProvider = filesProvider;
            this.fileService = fileService;
        }

        [HttpGet("user-uploaded/{filename}"), Authorize(Roles = Roles.Permissions.readUsers)]
        public async Task<IActionResult> GetUserFile(string filename, string? displayedName, CancellationToken cancellationToken)
        {
            var res = await participantService.GetParticipantFileAsync(filename, cancellationToken);
            if (!res.isSuccess)
                return Problem(res.error);            
            var filedata = res.value;
            filedata.fileName = displayedName is null ? filename : displayedName;
            return File(filedata);
        }

        [HttpGet("legal/{filename}")]
        public async Task<IActionResult> GetLegalFile(string filename, string? displayedName, CancellationToken cancellationToken)
        {
            var res = await filesProvider.DownloadLegalFileAsync(filename, cancellationToken);
            if (!res.isSuccess)
                return Problem(res.error);
            var filedata = res.value;
            filedata.fileName = displayedName is null ? filename : displayedName;
            return File(filedata);
        }

        [HttpPatch("legal"), Authorize(Roles = Roles.Permissions.utilsAccess)]
        public async Task<IActionResult> AppendLegalFiles([FromForm] AppendLegalFilesRequest request, CancellationToken cancellationToken)
        {
            await fileService.UploadLegalFilesAsync(request, cancellationToken);
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
            var Result = await filesProvider.DownloadCertificateBlankAsync(cancellationToken);
            if (!Result.isSuccess)
                return Problem(Result.error);
            return File(Result.value);
        }
        [HttpPatch("certificate"), Authorize(Roles = Roles.Permissions.utilsAccess)]
        public async Task<IActionResult> AppendCertificate(AppendCertificateRequest request, CancellationToken cancellationToken)
        {
            var res = await fileService.UploadCertificate(request, cancellationToken);
            return res.isSuccess? NoContent() : Problem(res.error);
        }
    }
}
