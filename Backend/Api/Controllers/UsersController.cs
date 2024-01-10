using Contracts.User;
using Domain.Services;
using Domain.Enumeration;
using Domain.Core.Primitives;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserService userService;
        private readonly IJWTProvider jwtProvider;

        public UsersController(ILogger<ApiController> logger, IUserService userService, IJWTProvider jwtProvider) : base(logger)
        {
            this.userService = userService;
            this.jwtProvider = jwtProvider;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var result = await userService.CheckIfUserExistsAsync(email, cancellationToken);
            return result.isSuccess ? Ok(result.value) : Problem(result.error);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewParticipant([FromForm] RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await userService.RegisterUserAsync(request, cancellationToken);
            if (!result.isSuccess)                
                return Problem(result.error);
            jwtProvider.IssueUserToken(Response, result.value, participant_status.Created);
            return CreatedAtAction(nameof(RegisterNewParticipant), result.value);
        }

        [HttpGet("authTest"), Authorize(Roles = )]
        public IActionResult TestAuth()
        {
            var nameId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            return Ok(nameId + " " + role);
        }
    }
}
