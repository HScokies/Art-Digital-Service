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

        //GET USER ROLES BASED ON TOKEN, CALL IT ON USE EFFECT INSIDE APP
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
        public IActionResult Logout()
        {
            jwtProvider.ClearToken();
            return NoContent();
        }

        [HttpGet("reset"), Authorize] //TODO
        public async Task<IActionResult> ResetPassword(CancellationToken cancellationToken)
        {
            await emailProvider.SendPasswordResetEmail(
                new MimeKit.MailboxAddress("Иван", "email@example.com"),
                resetUrl: $"https://example.com/{Guid.NewGuid()}",
                cancellationToken: cancellationToken
                );
            return Ok("sent");
        }

        [HttpGet("refresh"), Authorize]
        public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
        {
            var deviceIdResult = Request.GetDeviceId();
            if (!deviceIdResult.isSuccess) return Problem(deviceIdResult.error);

            var userIdResult = User.GetUserId();
            if (!userIdResult.isSuccess) return Problem(userIdResult.error);

            var res = await userService.RefreshTokenAsync(userIdResult.value, deviceIdResult.value, cancellationToken);
            return res.isSuccess? Ok() : Problem(res.error);
        }
        
    }
    
}
