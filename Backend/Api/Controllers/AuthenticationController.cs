using Api.Controllers.Base;
using Api.Controllers.Extensions;
using Application.Services.User;
using Contracts.User;
using Domain.Entities;
using Domain.Enumeration;
using Infrastructure.Authentication;
using Infrastructure.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    public class AuthenticationController : ApiController
    {
        private readonly IUserService userService;
        private readonly IJWTProvider jwtProvider;
        private readonly IEmailProvider emailProvider;
        public AuthenticationController(ILogger<ApiController> logger, IUserService userService, IJWTProvider jwtProvider, IEmailProvider emailProvider) : base(logger)
        {
            this.userService = userService;
            this.jwtProvider = jwtProvider;
            this.emailProvider = emailProvider;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewParticipant([FromForm] RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await userService.RegisterUserAsync(request, cancellationToken);
            if (!result.isSuccess)
                return Problem(result.error);

            return CreatedAtAction(nameof(RegisterNewParticipant), result.value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await userService.LoginUserAsync(request, cancellationToken);
            return result.isSuccess ? Ok(result.value) : Problem(result.error);
        }

        [HttpGet("logout"), Authorize]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            await jwtProvider.ClearToken(cancellationToken);
            return NoContent();
        }

        [HttpGet("refresh"), Authorize]
        public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
        {
            var deviceIdResult = Request.GetDeviceId();
            if (!deviceIdResult.isSuccess) return Problem(deviceIdResult.error);

            var userIdResult = User.GetUserId();
            if (!userIdResult.isSuccess) return Problem(userIdResult.error);

            var res = await userService.RefreshTokenAsync(userIdResult.value, deviceIdResult.value, cancellationToken);
            return res.isSuccess ? Ok(res.value) : Problem(res.error);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> RequestPasswordReset([FromForm] string email, CancellationToken cancellationToken)
        {

            var Result = await userService.RequestPasswordReset(email, cancellationToken);
            if (!Result.isSuccess)
                return Problem(Result.error);

            var user = Result.value;          
            await emailProvider.SendPasswordResetEmail(
                new MimeKit.MailboxAddress(user.name, user.email),
                resetToken: user.token,
                cancellationToken: cancellationToken
                );

            return NoContent();
        }

        [HttpPost("reset/{token}")]
        public async Task<IActionResult> ResetPassword(string token,[FromForm] string password, CancellationToken cancellationToken)
        {
            var Result = await userService.ResetPassword(token, password, cancellationToken);
            return Result.isSuccess? NoContent() : Problem(Result.error);
        }
    }
    
}
