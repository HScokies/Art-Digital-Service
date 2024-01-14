using Application.Services.User;
using Contracts.User;
using Domain.Enumeration;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserService userService;

        public UsersController(ILogger<ApiController> logger, IUserService userService) : base(logger)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var result = await userService.CheckIfUserExistsAsync(email, cancellationToken);
            return result.isSuccess ? Ok(result.value) : Problem(result.error);
        }
    }
}
