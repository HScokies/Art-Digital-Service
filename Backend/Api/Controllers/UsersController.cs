using Api.Controllers.Extensions;
using Application.Services.User;
using Contracts.User;
using Domain.Enumeration;
using Infrastructure.Authentication;
using Infrastructure.Emails;
using Infrastructure.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserService userService;
        private readonly IJWTProvider JWTProvider;
        private readonly IEmailProvider emailProvider;

        public UsersController(ILogger<ApiController> logger, IUserService userService, IJWTProvider JWTProvider, IEmailProvider emailProvider) : base(logger)
        {
            this.userService = userService;
            this.JWTProvider = JWTProvider;
            this.emailProvider = emailProvider;
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

            JWTProvider.IssueUserToken(response.userId, response.status);
            return Ok(response);
        }

        [HttpPost("append-files"), Authorize(Roles = Roles.ParticipantsStatus.sentPersonalData)]
        public async Task<IActionResult> AppendFilesAsync(IFormFile conscent, IFormFile solution)
        {
            var userIdResult = User.GetUserId();
            if (!userIdResult.isSuccess)
                return Problem(userIdResult.error);

            var Result = await userService.AppendParticipantFilesAsync(userIdResult.value,conscent, solution);
            if (!Result.isSuccess)
                return Problem(Result.error);

            var res = Result.value;
            JWTProvider.IssueUserToken(res.userId, res.status);
            return Ok(res);
        }
    }
}
