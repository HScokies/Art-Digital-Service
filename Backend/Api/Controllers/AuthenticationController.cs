using Application.Services.User;
using Contracts.User;
using Domain.Entities;
using Domain.Enumeration;
using Infrastructure.Authentication;
using Infrastructure.Emails;
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

        //TODO: Check if user exists, Register, LogIn, LogOut, Refresh token
        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewParticipant([FromForm] RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await userService.RegisterUserAsync(request, cancellationToken);
            if (!result.isSuccess)
                return Problem(result.error);
            jwtProvider.IssueUserToken(Response, result.value, Roles.ParticipantsStatus.justRegistered);
            return CreatedAtAction(nameof(RegisterNewParticipant), result.value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await userService.LoginUserAsync(request, cancellationToken);
            if (!result.isSuccess)
                return Problem(result.error);

            var user = result.value;
            if (user.Participant is not null)
                return LoginAsParticipant(user.Participant);

            if (user.Staff is not null)
                    return LoginAsStaff(user.Staff);

            return Problem(CommonErrors.Unknown);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            jwtProvider.ClearToken(Response);
            return NoContent();
        }


        [HttpGet("emailtest")]
        public async Task<IActionResult> Test(CancellationToken cancellationToken)
        {
            await emailProvider.SendPasswordResetEmail(
                recipientEmail: "email@example.com",
                recipientName: "Иван",
                resetUrl: $"https://example.com/{Guid.NewGuid()}",
                cancellationToken: cancellationToken
                );
            return Ok("sent");
        }

        private IActionResult LoginAsStaff(StaffDto staff)
        {
            jwtProvider.IssueStaffToken(Response, staff.userId, staff.Role.PermissionsList);
            return Ok();
        }

        private IActionResult LoginAsParticipant(ParticipantDto participant)
        {
            jwtProvider.IssueUserToken(Response, participant.userId, participant.status);
            return Ok();
        }
    }
}
