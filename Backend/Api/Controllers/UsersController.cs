using Api.Controllers.Extensions;
using Application.Services.User;
using Contracts.User;
using Domain.Enumeration;
using Infrastructure.Authentication;
using Infrastructure.Emails;
using Infrastructure.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserService userService;
        private readonly IJWTProvider jWTProvider;
        private readonly IEmailProvider emailProvider;
        private readonly IFilesService filesService;

        public UsersController(ILogger<ApiController> logger, IUserService userService, IJWTProvider JWTProvider, IEmailProvider emailProvider, IFilesService filesService) : base(logger)
        {
            this.userService = userService;
            jWTProvider = JWTProvider;
            this.emailProvider = emailProvider;
            this.filesService = filesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var result = await userService.CheckIfUserExistsAsync(email, cancellationToken);
            return result.isSuccess ? Ok(result.value) : Problem(result.error);
        }

        [HttpPost("append-data"), Authorize(Roles = Roles.ParticipantsStatus.justRegistered)]
        public async Task<IActionResult> AppendPersonalDataAsync([FromForm]PersonalDataAppendRequest request,CancellationToken cancellationToken)
        {
            var UserIdResult = User.GetUserId();
            if (!UserIdResult.isSuccess)
                return Problem(UserIdResult.error);

            var res = await userService.AppendParticipantDataAsync(UserIdResult.value, request, cancellationToken);
            if (!res.isSuccess)
                return Problem(res.error);

            var response = res.value;
            await emailProvider.SendWelcomeEmail(
                new MimeKit.MailboxAddress(
                    name: response.firstName,
                    address: response.email
                    ),
                youtubeId: response.youtubeId,
                continueUrl: $"https://example.com/{Guid.NewGuid()}",
                cancellationToken: cancellationToken
                );

            jWTProvider.IssueUserToken(response.userId, response.status);
            return Ok(response);
        }

        [HttpPost("append-files"), Authorize(Roles = Roles.ParticipantsStatus.sentPersonalData)]
        public IActionResult AppendFilesAsync(IFormFile conscent, IFormFile solution)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(conscent.FileName, out var contentType))
                return BadRequest();
            
            var ext = provider.get
            filesService.UploadFile(conscent,)
            return Ok("");
        }
    }
}
