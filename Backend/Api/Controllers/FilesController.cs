using Api.Controllers.Base;
using Application.Services.Participant;
using Domain.Core.Primitives;
using Domain.Enumeration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class FilesController : ApiController
    {
        private readonly IParticipantService participantService;

        public FilesController(ILogger<ApiController> logger, IParticipantService participantService) : base(logger)
        {
            this.participantService = participantService;
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
    }
}
